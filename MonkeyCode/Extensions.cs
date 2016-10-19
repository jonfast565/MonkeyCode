using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    static class Extensions
    {
        public static InstructionOpcode GetInstructionOpcode(this TokenType type)
        {
            switch (type)
            {
                case TokenType.BooleanTypeKeyword:
                    break;
                case TokenType.IntegerTypeKeyword:
                    break;
                case TokenType.StringTypeKeyword:
                    break;
                case TokenType.FloatTypeKeyword:
                    break;
                case TokenType.BooleanTrueLiteral:
                    break;
                case TokenType.BooleanFalseLiteral:
                    break;
                case TokenType.String:
                    break;
                case TokenType.Integer:
                    break;
                case TokenType.Identifier:
                    break;
                case TokenType.OperatorLeftParen:
                    break;
                case TokenType.OperatorRightParen:
                    break;
                case TokenType.OperatorSemicolon:
                    break;
                case TokenType.OperatorEquals:
                    break;
                case TokenType.OperatorNotEquals:
                    break;
                case TokenType.OperatorGreaterThan:
                    break;
                case TokenType.OperatorLessThan:
                    break;
                case TokenType.OperatorGreaterThanEqualTo:
                    break;
                case TokenType.OperatorLessThanEqualTo:
                    break;
                case TokenType.OperatorNot:
                    break;
                case TokenType.Plus:
                    return InstructionOpcode.Add;
                case TokenType.Minus:
                    return InstructionOpcode.Subtract;
                case TokenType.Multiply:
                    return InstructionOpcode.Multiply;
                case TokenType.Divide:
                    return InstructionOpcode.Divide;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return InstructionOpcode.None;
        }

        public static string GetIntelInstructionOpcode(this InstructionOpcode type)
        {
            const string result = "none";
            switch (type)
            {
                case InstructionOpcode.None:
                    break;
                case InstructionOpcode.Add:
                    return "add";
                case InstructionOpcode.Subtract:
                    return "sub";
                case InstructionOpcode.Multiply:
                    return "imul";
                case InstructionOpcode.Divide:
                    return "idiv";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return result;
        }
    }
}
