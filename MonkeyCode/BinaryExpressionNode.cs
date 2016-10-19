using System;
using System.Collections.Generic;

namespace MonkeyCode
{
    internal class BinaryExpressionNode : ISemanticObject
    {
        public BinaryExpressionNode(Token token, bool isOperator, IValue value = null)
        {
            Token = token;
            Value = value;
            IsOperator = isOperator;
            LeftChild = null;
            RightChild = null;
            Intermediate = Guid.NewGuid().ToString().Substring(0, 8);
        }

        public BinaryExpressionNode LeftChild { get; set; }
        public BinaryExpressionNode RightChild { get; set; }

        public Token Token { get; set; }

        public IValue Value { get; set; }

        public bool IsOperator { get; set; }

        public string Intermediate { get; set; }

        public List<Instruction> AppendInstructions(List<Instruction> instructionList)
        {
            return GetInstructionsInternal(this, instructionList); 
        }

        private List<Instruction> GetInstructionsInternal(BinaryExpressionNode node, List<Instruction> instructionList)
        {
            if (node.LeftChild.IsOperator)
            {
                GetInstructionsInternal(node.LeftChild, instructionList);
            }

            if (node.RightChild.IsOperator)
            {
                GetInstructionsInternal(node.RightChild, instructionList);
            }

            // only operator needed, access one below for folding
            if (!node.IsOperator) return instructionList;

            instructionList.Add(new Instruction
            {
                Opcode = node.Token.Type.GetInstructionOpcode(),
                Value1 = node.RightChild.IsOperator
                    ? new Identifier { Name = node.RightChild.Intermediate }
                    : (IValue)new IntegerLiteral { Value = Convert.ToInt32(node.RightChild.Token.Lexeme) },
                Value2 = node.LeftChild.IsOperator
                    ? new Identifier { Name = node.LeftChild.Intermediate }
                    : (IValue)new IntegerLiteral { Value = Convert.ToInt32(node.LeftChild.Token.Lexeme) },
                Result = new Identifier { Name = node.Intermediate }
            });

            return instructionList;
        }
    }
}