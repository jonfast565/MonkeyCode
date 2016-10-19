using System;
using System.Collections.Generic;

namespace MonkeyCode
{
    internal class InstructionBuilder
    {
        private readonly IEnumerable<ISemanticObject> semanticBlockList;

        public InstructionBuilder(IEnumerable<ISemanticObject> semanticBlockList)
        {
            this.semanticBlockList = semanticBlockList;
        }

        public List<Instruction> Build()
        {
            var instructionList = new List<Instruction>();
            foreach (var block in semanticBlockList)
            {
                if (block.GetType() == typeof(BinaryExpressionNode))
                {
                    ((BinaryExpressionNode) block).AppendInstructions(instructionList);
                }
            }

            return instructionList;
        }
    }
}