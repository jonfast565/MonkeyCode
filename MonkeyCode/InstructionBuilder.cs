using System;
using System.Collections.Generic;

namespace MonkeyCode
{
    internal class InstructionBuilder
    {
        private readonly IEnumerable<ISemanticObject> _semanticBlockList;

        public InstructionBuilder(IEnumerable<ISemanticObject> semanticBlockList)
        {
            _semanticBlockList = semanticBlockList;
        }

        public List<Instruction> Build()
        {
            var instructionList = new List<Instruction>();

            foreach (var block in _semanticBlockList)
            {
                if (block.GetType() == typeof(AssignmentStatement))
                {
                    ((AssignmentStatement)block).AppendInstructions(instructionList);
                }
            }

            return instructionList;
        }
    }
}