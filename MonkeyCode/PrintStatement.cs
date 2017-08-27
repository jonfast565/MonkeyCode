using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    public class PrintStatement : ISemanticObject
    {
        public Identifier Identifier { get; internal set; }

        public List<Instruction> AppendInstructions(List<Instruction> instructionList)
        {
            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Push,
                Target = Identifier
            });
            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Call,
                // Value1 = new
            });
            return instructionList;
        }
    }
}
