using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    public class AssignmentStatement : ISemanticObject
    {
        public BinaryExpressionNode Expression { get; set; }
        public Identifier Result { get; set; }

        public List<Instruction> AppendInstructions(List<Instruction> instructionList)
        {
            var expressionId = Expression.Intermediate;
            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Allocate,
                Source = Result,
                Target = Result
            });
            Expression.AppendInstructions(instructionList);
            instructionList.Add(new Instruction
            {
                Opcode = InstructionOpcode.Move,
                Source = new Identifier { Name = expressionId },
                Target = Result
            });
            return instructionList;
        }
    }
}
