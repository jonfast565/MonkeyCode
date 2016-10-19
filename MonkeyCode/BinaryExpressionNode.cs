using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class BinaryExpressionNode : ISemanticObject
    {
        public BinaryExpressionNode LeftChild { get; set; }
        public BinaryExpressionNode RightChild { get; set; } 

        public Token Token { get; set; }

        public IValue Value { get; set; }

        public bool IsOperator { get; set; }

        public string Intermediate { get; set; }

        public BinaryExpressionNode(Token token, bool isOperator, IValue value = null)
        {
            Token = token;
            Value = value;
            IsOperator = isOperator;
            LeftChild = null;
            RightChild = null; 
            Intermediate = Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
