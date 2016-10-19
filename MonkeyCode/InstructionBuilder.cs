using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class InstructionBuilder
    {
        private IEnumerable<ISemanticObject> semanticBlockList;

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
                    GenerateBinaryExpressionNode((BinaryExpressionNode)block, instructionList);
                }
            }

            return instructionList;
        }

        public List<Instruction> GenerateBinaryExpressionNode(BinaryExpressionNode node, List<Instruction> instructionList)
        {
            if (node.LeftChild.IsOperator)
            {
                GenerateBinaryExpressionNode(node.LeftChild, instructionList);
            }

            if (node.RightChild.IsOperator)
            {
                GenerateBinaryExpressionNode(node.RightChild, instructionList);
            }

            // only operator needed, access one below for folding
            if (!node.IsOperator) return instructionList;

            instructionList.Add(new Instruction
            {
                Opcode = node.Token.Type.GetInstructionOpcode(),
                Value1 = node.RightChild.IsOperator ? 
                            new Identifier { Name = node.RightChild.Intermediate } : 
                            (IValue) new IntegerLiteral { Value = Convert.ToInt32(node.RightChild.Token.Lexeme) },
                Value2 = node.LeftChild.IsOperator ? 
                            new Identifier { Name = node.LeftChild.Intermediate } : 
                            (IValue) new IntegerLiteral { Value = Convert.ToInt32(node.LeftChild.Token.Lexeme) },
                Result = new Identifier { Name = node.Intermediate }
            });

            return instructionList;
        }
    }
}
