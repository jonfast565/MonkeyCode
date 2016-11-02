using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonkeyCode
{
    internal class IntelGenerator
    {
        public IntelGenerator(List<Instruction> instructionList)
        {
            InstructionList = instructionList;
            SymbolTable = new Dictionary<string, Symbol>();
            StackOffset = 1;
        }

        public List<Instruction> InstructionList { get; set; }

        public IDictionary<string, Symbol> SymbolTable { get; set; }

        public const int IntegerOffset = 4;

        public int StackOffset { get; set; }

        public string LoadValue1(Instruction instruction)
        {
            if (instruction.Opcode == InstructionOpcode.Add
                || instruction.Opcode == InstructionOpcode.Divide
                || instruction.Opcode == InstructionOpcode.Multiply
                || instruction.Opcode == InstructionOpcode.Subtract)
            {
                // load value to manip
                if (instruction.Value1.GetType() == typeof(Identifier))
                {
                    var sym = SymbolTable[instruction.Value1.GetValue()];
                    return "\tmov eax, " + "[ebp - " + sym.Offset + "]\r\n";
                }

                if (instruction.Value1.GetType() == typeof(IntegerLiteral))
                {
                    return "\tmov eax, " + instruction.Value1.GetValue() + "\r\n";
                }
            }

            return string.Empty;
        }

        public string PerformOpOnValue2(Instruction instruction)
        {
            if (instruction.Opcode == InstructionOpcode.Add
                || instruction.Opcode == InstructionOpcode.Divide
                || instruction.Opcode == InstructionOpcode.Multiply
                || instruction.Opcode == InstructionOpcode.Subtract)
            {
                if (instruction.Value2.GetType() == typeof(Identifier))
                {
                    var sym = SymbolTable[instruction.Value2.GetValue()];
                    if (instruction.Opcode == InstructionOpcode.Multiply
                        || instruction.Opcode == InstructionOpcode.Divide)
                    {
                        return "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                               + " dword " + "[ebp - " + sym.Offset + "]\r\n";
                    }

                    if (instruction.Opcode == InstructionOpcode.Add
                        || instruction.Opcode == InstructionOpcode.Subtract)
                    {
                        return "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                               + " eax, " + "[ebp - " + sym.Offset + "]\r\n";
                    }
                }




                if (instruction.Value2.GetType() == typeof(IntegerLiteral))
                {
                    if (instruction.Opcode == InstructionOpcode.Divide
                        || instruction.Opcode == InstructionOpcode.Multiply)
                    {
                        return "\t" + "mov ebx, " + instruction.Value2.GetValue() + "\r\n"
                               + "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                               + " ebx\r\n"
                               + "\txor ebx, ebx\r\n";
                    }

                    if (instruction.Opcode == InstructionOpcode.Add
                        || instruction.Opcode == InstructionOpcode.Subtract)
                    {
                        return "\t" + instruction.Opcode.GetIntelInstructionOpcode()
                               + " eax, " + instruction.Value2.GetValue() + "\r\n";
                    }
                }
            }

            return string.Empty;
        }

        public string IntermediateProcessing(Instruction instruction)
        {
            if (instruction.Opcode == InstructionOpcode.Multiply ||
                    instruction.Opcode == InstructionOpcode.Divide)
            {
                return "\tcdq\r\n"; // extend to edx:eax
            }

            return string.Empty;
        }

        public string StoreResultValue(Instruction instruction)
        {
            if (instruction.Opcode == InstructionOpcode.Add
                || instruction.Opcode == InstructionOpcode.Subtract
                || instruction.Opcode == InstructionOpcode.Multiply
                || instruction.Opcode == InstructionOpcode.Divide)
            {
                SymbolTable.Add(instruction.Target.Name, new Symbol
                {
                    Location = SymbolLocation.Stack,
                    Name = instruction.Target.Name,
                    Offset = StackOffset*IntegerOffset
                });
                StackOffset++;
                return "\tpush eax\r\n";
            }

            if (instruction.Opcode == InstructionOpcode.Allocate)
            {
                SymbolTable.Add(instruction.Source.Name, new Symbol
                {
                    Location = SymbolLocation.Stack,
                    Name = instruction.Source.Name,
                    Offset = StackOffset * IntegerOffset
                });
                StackOffset++;
                return "\tpush 0\r\n";
            }

            return string.Empty;
        }


        public string PopResultValueIntoLoc(Instruction instruction)
        {
            if (instruction.Opcode != InstructionOpcode.Move) return string.Empty;
            var symTarget = SymbolTable[instruction.Target.GetValue()];
            var symSource = SymbolTable[instruction.Source.GetValue()];
            return "\tmov eax, [ebp - " + symSource.Offset + "]\r\n" 
                + "\tmov [ebp - " + symTarget.Offset + "], eax\r\n";
        }

        public string RewindStack()
        {
            return "\tadd esp, " + (StackOffset - 1) * IntegerOffset + "\r\n";
        }

        public string GetProgramPrologue()
        {
            return @"
%include ""io.inc""
section .text
global CMAIN
CMAIN:
";
        }

        public string GenerateCode()
        {
            var result = GetProgramPrologue();
            result += GenerateFunctionPrologue();
            foreach (var instruction in InstructionList)
            {
                result += LoadValue1(instruction);
                result += IntermediateProcessing(instruction);
                result += PerformOpOnValue2(instruction);
                result += StoreResultValue(instruction);
                result += PopResultValueIntoLoc(instruction);
            }
            result += RewindStack();
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