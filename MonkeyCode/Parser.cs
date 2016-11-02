using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyCode
{
    internal class Parser
    {
        public Parser(List<Token> tokenList)
        {
            TokenList = tokenList;
            TokenPointer = 0;
        }

        public List<Token> TokenList { get; set; }

        public int TokenPointer { get; set; }

        public IEnumerable<ISemanticObject> Parse()
        {
            var exprList = new List<ISemanticObject>();
            while (TokenPointer < TokenList.Count)
            {
                ISemanticObject s;
                try
                {
                    s = ParseAssignmentStatement();
                    Match(TokenType.OperatorSemicolon);
                }
                catch (Exception e)
                {
                    throw new Exception("Could not be parsed", e);
                }
                exprList.Add(s);
            }
            return exprList;
        }

        public ISemanticObject ParseAssignmentStatement()
        {
            var id = Match(TokenType.Identifier);
            Match(TokenType.OperatorEquals);
            var savedPointer = TokenPointer;
            ValidateExpression();
            var parsed = TokenList
                .Skip(savedPointer)
                .Take(TokenPointer - savedPointer);
            var tokens = ToPostOrder(parsed
                .Where(x => x.Type != TokenType.OperatorSemicolon));
            var opStack = new Stack<BinaryExpressionNode>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Plus ||
                    token.Type == TokenType.Minus ||
                    token.Type == TokenType.Multiply ||
                    token.Type == TokenType.Divide)
                {
                    var o = new BinaryExpressionNode(token, true);
                    var operands = new List<BinaryExpressionNode>();
                    for (var i = 0; i < 2; i++)
                    {
                        if (opStack.Any())
                        {
                            operands.Add(opStack.Pop());
                        }
                    }
                    o.LeftChild = operands[0];
                    o.RightChild = operands[1];
                    opStack.Push(o);
                }
                else
                {
                    opStack.Push(new BinaryExpressionNode(token, false));
                }
            }

            return new AssignmentStatement
            {
                Result = new Identifier
                {
                    Name = id.Lexeme
                },
                Expression = opStack.Pop()
            };
        }

        private Token Match(TokenType t1)
        {
            if (!CondMatch(t1))
            {
                throw new Exception($"No matching token {t1}");
            }
            var token = TokenList[TokenPointer];
            TokenPointer++;
            return token;
        }

        private bool CondMatch(TokenType t1)
        {
            if (TokenPointer < TokenList.Count)
            {
                return t1 == TokenList[TokenPointer].Type;
            }
            return false;
        }

        private void ValidateExpression()
        {
            ValidateHigherPrecedenceExpression();
            ValidateAdditionSubtraction();
        }

        private void ValidateHigherPrecedenceExpression()
        {
            ValidateParentheticalSubExpression();
            ValidateMultiplicationDivision();
        }

        private void ValidateAdditionSubtraction()
        {
            if (CondMatch(TokenType.Plus))
            {
                Match(TokenType.Plus);
                ValidateHigherPrecedenceExpression();
                ValidateAdditionSubtraction();
            }
            else if (CondMatch(TokenType.Minus))
            {
                Match(TokenType.Minus);
                ValidateHigherPrecedenceExpression();
                ValidateAdditionSubtraction();
            }
        }

        private void ValidateMultiplicationDivision()
        {
            if (CondMatch(TokenType.Multiply))
            {
                Match(TokenType.Multiply);
                ValidateParentheticalSubExpression();
                ValidateMultiplicationDivision();
            }
            else if (CondMatch(TokenType.Divide))
            {
                Match(TokenType.Divide);
                ValidateParentheticalSubExpression();
                ValidateMultiplicationDivision();
            }
        }

        private void ValidateParentheticalSubExpression()
        {
            if (!CondMatch(TokenType.Integer) && !CondMatch(TokenType.Identifier))
            {
                Match(TokenType.OperatorLeftParen);
                ValidateExpression();
                Match(TokenType.OperatorRightParen);
            }
            else if (!CondMatch(TokenType.Integer))
            {
                Match(TokenType.Identifier);
            }
            else
            {
                Match(TokenType.Integer);
            }
        }

        private static int GetOperatorPrecendence(TokenType type)
        {
            switch (type)
            {
                case TokenType.OperatorLeftParen:
                    return 0;
                case TokenType.Plus:
                case TokenType.Minus:
                    return 3;
                case TokenType.Multiply:
                case TokenType.Divide:
                    return 4;
                default:
                    throw new Exception("Operator not recognized");
            }
        }

        private IEnumerable<Token> ToPostOrder(IEnumerable<Token> tokenList)
        {
            var list = tokenList.ToList();
            var opStack = new Stack<Token>();
            var output = new List<Token>();

            foreach (var token in list)
            {
                switch (token.Type)
                {
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Multiply:
                    case TokenType.Divide:
                        while (opStack.Any())
                        {
                            var top = opStack.Peek();
                            if (GetOperatorPrecendence(top.Type) <
                                GetOperatorPrecendence(token.Type))
                            {
                                break;
                            }
                            output.Add(opStack.Pop());
                        }
                        opStack.Push(token);
                        break;
                    case TokenType.OperatorLeftParen:
                        opStack.Push(token);
                        break;
                    case TokenType.OperatorRightParen:
                        while (opStack.Any())
                        {
                            var top = opStack.Pop();
                            if (top.Type != TokenType.OperatorLeftParen)
                            {
                                output.Add(top);
                            }
                            else
                            {
                                // found left paren
                                break;
                            }
                        }
                        break;
                    default:
                        // operator
                        output.Add(token);
                        break;
                }
            }
            while (opStack.Any())
            {
                output.Add(opStack.Pop());
            }
            return output;
        }
    }
}