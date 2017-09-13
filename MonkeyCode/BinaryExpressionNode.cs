using System;
using System.Collections.Generic;

namespace MonkeyCode
{
    public class BinaryExpressionNode : ISemanticObject
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
            if (node.LeftChild != null && node.LeftChild.IsOperator)
            {
                GetInstructionsInternal(node.LeftChild, instructionList);
            }

            if (node.RightChild != null && node.RightChild.IsOperator)
            {
                GetInstructionsInternal(node.RightChild, instructionList);
            }

            if (!node.IsOperator) return instructionList;

            instructionList.Add(new Instruction
            {
                Opcode = node.Token.Type.GetInstructionOpcode(),
                Value1 = node.RightChild.IsOperator
                    ? new Identifier { Name = node.RightChild.Intermediate }
                    : GetConvertedTokenValue(node.RightChild.Token),
                Value2 = node.LeftChild.IsOperator
                    ? new Identifier { Name = node.LeftChild.Intermediate }
                    : GetConvertedTokenValue(node.LeftChild.Token), 
                Target = new Identifier { Name = node.Intermediate }
            });

            return instructionList;
        }

        private IValue GetConvertedTokenValue(Token token) 
        {
            if (token.Type == TokenType.Integer)
            {
                return new IntegerLiteral {Value = Convert.ToInt32(token.Lexeme)};
            }
            if (token.Type == TokenType.Identifier)
            {
                return new Identifier {Name = token.Lexeme};
            }
            throw new Exception("Unsupported value conversion in expression");
        }
    }
}