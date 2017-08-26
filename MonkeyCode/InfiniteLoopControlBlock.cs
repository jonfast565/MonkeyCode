using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    public class InfiniteLoopControlBlock : ISemanticObject
    {
        public List<ISemanticObject> InnerExpressions { get; set; }

        public static int LoopNumber { get; set; }

        public InfiniteLoopControlBlock(List<ISemanticObject> innerExpressions)
        {
            InnerExpressions = innerExpressions;
            LoopNumber++;
        }

        public List<Instruction> AppendInstructions(List<Instruction> instructionList)
        {
            var loopBeginJumpIdentifier = new Identifier { Name = "L" + LoopNumber + "_Begin", Temporary = true };
            var loopEndJumpIdentifier = new Identifier { Name = "L" + LoopNumber + "_End", Temporary = true };

            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Label,
                Target = loopBeginJumpIdentifier
            });

            foreach (var expression in InnerExpressions)
            {
                expression.AppendInstructions(instructionList);
            }

            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Jump,
                Target = loopBeginJumpIdentifier
            });

            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Label,
                Target = loopEndJumpIdentifier
            });

            return instructionList;
        }
    }
}
