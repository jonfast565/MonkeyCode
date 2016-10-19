using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class IntelGenerator
    {
        public IntelGenerator(List<Instruction> instructionList)
        {
            InstructionList = instructionList;
        }

        public List<Instruction> InstructionList { get; set; }

        public string GenerateCode()
        {
            var result =
@"
%include ""io.inc""
section .text
global CMAIN
CMAIN:
";
            result += GenerateFunctionPrologue();

            // var symbols = new Dictionary<string, Symbol>();
            var cheapOffset = 1;
            var intOffsetFactor = 4;

            var symbolDict = new Dictionary<string, Symbol>();
            var counter = 0;
            foreach (var instruction in InstructionList)
            {
                if (instruction.Value1.GetType() == typeof(Identifier))
                {
                    var sym = symbolDict[instruction.Value1.GetValue()];
                    result += "\tmov eax, " + "[ebp - " + sym.Offset + "]\r\n";
                }
                else if (instruction.Value1.GetType() == typeof(IntegerLiteral))
                {
                    result += "\tmov eax, " + instruction.Value1.GetValue() + "\r\n";
                }

                if (instruction.Opcode == InstructionOpcode.Multiply ||
                    instruction.Opcode == InstructionOpcode.Divide)
                {
                    result += "\tcdq\r\n"; // extend to edx:eax
                }

                if (instruction.Value2.GetType() == typeof(Identifier))
                {
                    var sym = symbolDict[instruction.Value2.GetValue()];
                    if (instruction.Opcode == InstructionOpcode.Multiply
                        || instruction.Opcode == InstructionOpcode.Divide)
                    {
                        result += "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                            + "[ebp - " + sym.Offset + "]\r\n";
                    }
                    else
                    {
                        result += "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                                  + " eax, " + "[ebp - " + sym.Offset + "]\r\n";
                    }
                }
                else if (instruction.Value2.GetType() == typeof(IntegerLiteral))
                {
                    if (instruction.Opcode == InstructionOpcode.Divide
                        || instruction.Opcode == InstructionOpcode.Multiply)
                    {
                        result += "\t" + "mov ebx, " + instruction.Value2.GetValue() + "\r\n";
                        result += "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                                  + " ebx\r\n";
                        result += "\txor ebx, ebx\r\n";
                    }
                    else
                    {
                        result += "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                                  + " eax, " + instruction.Value2.GetValue() + "\r\n";
                    }
                }

                if (counter < InstructionList.Count - 1)
                {
                    result += "\tpush eax\r\n";
                    symbolDict.Add(instruction.Result.Name, new Symbol
                    {
                        Location = SymbolLocation.Stack,
                        Name = instruction.Result.Name,
                        Offset = cheapOffset*intOffsetFactor
                    });
                    cheapOffset++;
                }
                else
                {
                    if (cheapOffset*intOffsetFactor > 4)
                    {
                        result += "\tadd esp, " + (cheapOffset - 1)*intOffsetFactor + "\r\n";
                    }
                }

                counter++;
            }
            result += "\tPRINT_DEC 4, eax\r\n";
            result += GenerateFunctionEpilogue();
            return result;
        }

        private string GenerateFunctionPrologue()
        {
            var result = "\tpush ebp\r\n";
            result += "\tmov ebp, esp\r\n";
            return result;
        }

        private string GenerateFunctionEpilogue()
        {
            var result = "\tmov esp, ebp\r\n";
            result += "\tpop ebp\r\n";
            result += "\tret\r\n";
            return result;
        }
    }
}
