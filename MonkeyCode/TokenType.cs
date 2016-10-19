using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    enum TokenType
    {
        BooleanTypeKeyword,
        IntegerTypeKeyword,
        StringTypeKeyword,
        FloatTypeKeyword,
        BooleanTrueLiteral,
        BooleanFalseLiteral,
        String,
        Integer,
        Identifier,
        OperatorLeftParen,
        OperatorRightParen,
        OperatorSemicolon,
        OperatorEquals,
        OperatorNotEquals,
        OperatorGreaterThan,
        OperatorLessThan,
        OperatorGreaterThanEqualTo,
        OperatorLessThanEqualTo,
        OperatorNot,
        Plus,
        Minus,
        Multiply,
        Divide,
    }
}
