using System;

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
    }
}