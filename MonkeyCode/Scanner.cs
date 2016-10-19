using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class Scanner
    {
        public Scanner(string inputString)
        {
            ScanBuffer = inputString + " \0";
            ScanPointer = 0;
            Column = 1;
            Line = 1;
        }

        private string ScanBuffer { get; }
        private int ScanPointer { get; set; }
        private int Line { get; set; }
        private int Column { get; set; }

        public List<Token> Scan()
        {
            return ScanAll().Where(x => x != null).ToList();
        }

        private bool IsBreakChar(char character)
        {
            return IsSymbol(character)
                   || IsWhitespace(character)
                   || character == '\0';
        }

        private static bool IsSymbol(char character)
        {
            return character == '\''
                   || character == '('
                   || character == ')'
                   || character == ','
                   || character == '.'
                   || character == ';'
                   || character == '\''
                   || character == '/'
                   || character == '@';
        }

        private bool IsWhitespace(char character)
        {
            if (character == '\n')
            {
                Line++;
                Column = 0;
            }
            return character == ' '
                   || character == '\t'
                   || character == '\n'
                   || character == '\r'
                   || character == '\v';
        }

        private void ScanWhitespace()
        {
            while (IsWhitespace(ScanBuffer[ScanPointer]))
            {
                ScanPointer++;
            }
        }

        public IEnumerable<Token> ScanAll()
        {
            var tokenList = new List<Token>();
            while (true)
            {
                var result = ScanOne();
                if (result == null) break;
                tokenList.Add(result);
            }
            return tokenList;
        }

        private bool ScanBooleanTypeKeyword()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != 'B' && ScanBuffer[ScanPointer + 0] != 'b') { return false; }
                if (ScanBuffer[ScanPointer + 1] != 'O' && ScanBuffer[ScanPointer + 1] != 'o') { return false; }
                if (ScanBuffer[ScanPointer + 2] != 'O' && ScanBuffer[ScanPointer + 2] != 'o') { return false; }
                if (ScanBuffer[ScanPointer + 3] != 'L' && ScanBuffer[ScanPointer + 3] != 'l') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 4])) { return false; }
                ScanPointer += 4;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private bool ScanStringTypeKeyword()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != 'S' && ScanBuffer[ScanPointer + 0] != 's') { return false; }
                if (ScanBuffer[ScanPointer + 1] != 'T' && ScanBuffer[ScanPointer + 1] != 't') { return false; }
                if (ScanBuffer[ScanPointer + 2] != 'R' && ScanBuffer[ScanPointer + 2] != 'r') { return false; }
                if (ScanBuffer[ScanPointer + 3] != 'I' && ScanBuffer[ScanPointer + 3] != 'i') { return false; }
                if (ScanBuffer[ScanPointer + 4] != 'N' && ScanBuffer[ScanPointer + 4] != 'n') { return false; }
                if (ScanBuffer[ScanPointer + 5] != 'G' && ScanBuffer[ScanPointer + 5] != 'g') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 6])) { return false; }
                ScanPointer += 6;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanFloatTypeKeyword()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != 'F' && ScanBuffer[ScanPointer + 0] != 'f') { return false; }
                if (ScanBuffer[ScanPointer + 1] != 'L' && ScanBuffer[ScanPointer + 1] != 'l') { return false; }
                if (ScanBuffer[ScanPointer + 2] != 'O' && ScanBuffer[ScanPointer + 2] != 'o') { return false; }
                if (ScanBuffer[ScanPointer + 3] != 'A' && ScanBuffer[ScanPointer + 3] != 'a') { return false; }
                if (ScanBuffer[ScanPointer + 4] != 'T' && ScanBuffer[ScanPointer + 4] != 't') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 5])) { return false; }
                ScanPointer += 5;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanIntegerTypeKeyword()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != 'I' && ScanBuffer[ScanPointer + 0] != 'i') { return false; }
                if (ScanBuffer[ScanPointer + 1] != 'N' && ScanBuffer[ScanPointer + 1] != 'n') { return false; }
                if (ScanBuffer[ScanPointer + 2] != 'T' && ScanBuffer[ScanPointer + 2] != 't') { return false; }
                if (ScanBuffer[ScanPointer + 3] != 'E' && ScanBuffer[ScanPointer + 3] != 'e') { return false; }
                if (ScanBuffer[ScanPointer + 4] != 'G' && ScanBuffer[ScanPointer + 4] != 'g') { return false; }
                if (ScanBuffer[ScanPointer + 5] != 'E' && ScanBuffer[ScanPointer + 5] != 'e') { return false; }
                if (ScanBuffer[ScanPointer + 6] != 'R' && ScanBuffer[ScanPointer + 6] != 'r') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 7])) { return false; }
                ScanPointer += 7;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorLeftParen()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '(' && ScanBuffer[ScanPointer + 0] != '(') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorRightParen()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != ')' && ScanBuffer[ScanPointer + 0] != ')') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorSemicolon()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != ';' && ScanBuffer[ScanPointer + 0] != ';') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorEquals()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '=' && ScanBuffer[ScanPointer + 0] != '=') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorNotEquals()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '<' && ScanBuffer[ScanPointer + 0] != '<') { return false; }
                if (ScanBuffer[ScanPointer + 1] != '>' && ScanBuffer[ScanPointer + 1] != '>') { return false; }
                ScanPointer += 2;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorGreaterThan()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '>' && ScanBuffer[ScanPointer + 0] != '>') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorLessThan()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '<' && ScanBuffer[ScanPointer + 0] != '<') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 1])) { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorGreaterThanEqualTo()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '>' && ScanBuffer[ScanPointer + 0] != '>') { return false; }
                if (ScanBuffer[ScanPointer + 1] != '=' && ScanBuffer[ScanPointer + 1] != '=') { return false; }
                ScanPointer += 2;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorLessThanEqualTo()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '<' && ScanBuffer[ScanPointer + 0] != '<') { return false; }
                if (ScanBuffer[ScanPointer + 1] != '=' && ScanBuffer[ScanPointer + 1] != '=') { return false; }
                ScanPointer += 2;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorNot()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '!' && ScanBuffer[ScanPointer + 0] != '!') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanBooleanTrueLiteral()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != 'T' && ScanBuffer[ScanPointer + 0] != 't') { return false; }
                if (ScanBuffer[ScanPointer + 1] != 'R' && ScanBuffer[ScanPointer + 1] != 'r') { return false; }
                if (ScanBuffer[ScanPointer + 2] != 'U' && ScanBuffer[ScanPointer + 2] != 'u') { return false; }
                if (ScanBuffer[ScanPointer + 3] != 'E' && ScanBuffer[ScanPointer + 3] != 'e') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 5])) { return false; }
                ScanPointer += 4;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanBooleanFalseLiteral()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != 'F' && ScanBuffer[ScanPointer + 0] != 'f') { return false; }
                if (ScanBuffer[ScanPointer + 1] != 'A' && ScanBuffer[ScanPointer + 1] != 'a') { return false; }
                if (ScanBuffer[ScanPointer + 2] != 'L' && ScanBuffer[ScanPointer + 2] != 'l') { return false; }
                if (ScanBuffer[ScanPointer + 3] != 'S' && ScanBuffer[ScanPointer + 3] != 's') { return false; }
                if (ScanBuffer[ScanPointer + 4] != 'E' && ScanBuffer[ScanPointer + 4] != 'e') { return false; }
                if (IsIdentifierChar(ScanBuffer[ScanPointer + 5])) { return false; }
                ScanPointer += 5;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private bool ScanOperatorPlus()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '+' && ScanBuffer[ScanPointer + 0] != '+') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorMinus()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '-' && ScanBuffer[ScanPointer + 0] != '-') { return false; }
                if (IsNumericChar(ScanBuffer[ScanPointer + 1])) { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorMultiply()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '*' && ScanBuffer[ScanPointer + 0] != '*') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        private bool ScanOperatorDivide()
        {
            try
            {
                if (ScanBuffer[ScanPointer + 0] != '/' && ScanBuffer[ScanPointer + 0] != '/') { return false; }
                ScanPointer += 1;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private Token ScanOne()
        {
            // whitespace
            ScanWhitespace();

            // boolean operators
            if (ScanOperatorLeftParen())
            {
                return new Token(TokenType.OperatorLeftParen);
            }
            if (ScanOperatorRightParen())
            {
                return new Token(TokenType.OperatorRightParen);
            }
            if (ScanOperatorSemicolon())
            {
                return new Token(TokenType.OperatorSemicolon);
            }
            if (ScanOperatorNotEquals())
            {
                return new Token(TokenType.OperatorNotEquals);
            }
            if (ScanOperatorGreaterThanEqualTo())
            {
                return new Token(TokenType.OperatorGreaterThanEqualTo);
            }
            if (ScanOperatorLessThanEqualTo())
            {
                return new Token(TokenType.OperatorLessThanEqualTo);
            }
            if (ScanOperatorEquals())
            {
                return new Token(TokenType.OperatorEquals);
            }
            if (ScanOperatorGreaterThan())
            {
                return new Token(TokenType.OperatorGreaterThan);
            }
            if (ScanOperatorLessThan())
            {
                return new Token(TokenType.OperatorLessThan);
            }
            if (ScanOperatorNot())
            {
                return new Token(TokenType.OperatorNot);
            }

            if (ScanOperatorPlus())
            {
                return new Token(TokenType.Plus);
            }
            if (ScanOperatorMinus())
            {
                return new Token(TokenType.Minus);
            }
            if (ScanOperatorMultiply())
            {
                return new Token(TokenType.Multiply);
            }
            if (ScanOperatorDivide())
            {
                return new Token(TokenType.Divide);
            }

            // keywords
            if (ScanBooleanTypeKeyword())
            {
                return new Token(TokenType.BooleanTypeKeyword);
            }
            if (ScanIntegerTypeKeyword())
            {
                return new Token(TokenType.IntegerTypeKeyword);
            }
            if (ScanStringTypeKeyword())
            {
                return new Token(TokenType.StringTypeKeyword);
            }
            if (ScanFloatTypeKeyword())
            {
                return new Token(TokenType.FloatTypeKeyword);
            }

            // literals
            if (ScanBooleanTrueLiteral())
            {
                return new Token(TokenType.BooleanTrueLiteral);
            }
            if (ScanBooleanFalseLiteral())
            {
                return new Token(TokenType.BooleanFalseLiteral);
            }

            // scan string
            string value = null;
            if (ScanString(out value))
            {
                return new Token(TokenType.String, value);
            }

            // scan integer
            value = null;
            if (ScanInteger(out value))
            {
                return new Token(TokenType.Integer, value);
            }

            // scan identifier (usually a last resort)
            value = null;
            if (ScanIdentifier(out value))
            {
                return new Token(TokenType.Identifier, value);
            }

            // end of file
            if (ScanBuffer[ScanPointer + 0] == '\0') return null;

            throw new ScanException(
                $"Unclassifiable character found @ loc {ScanPointer + 1}: '{ScanBuffer[ScanPointer + 0]}' on line " +
                Line);
            // return null;
        }

        private static bool IsIdentifierChar(char character)
        {
            return character == 'A' || character == 'a'
                   || character == 'B' || character == 'b'
                   || character == 'C' || character == 'c'
                   || character == 'D' || character == 'd'
                   || character == 'E' || character == 'e'
                   || character == 'F' || character == 'f'
                   || character == 'G' || character == 'g'
                   || character == 'H' || character == 'h'
                   || character == 'I' || character == 'i'
                   || character == 'J' || character == 'j'
                   || character == 'K' || character == 'k'
                   || character == 'L' || character == 'l'
                   || character == 'M' || character == 'm'
                   || character == 'N' || character == 'n'
                   || character == 'O' || character == 'o'
                   || character == 'P' || character == 'p'
                   || character == 'Q' || character == 'q'
                   || character == 'R' || character == 'r'
                   || character == 'S' || character == 's'
                   || character == 'T' || character == 't'
                   || character == 'U' || character == 'u'
                   || character == 'V' || character == 'v'
                   || character == 'W' || character == 'w'
                   || character == 'X' || character == 'x'
                   || character == 'Y' || character == 'y'
                   || character == 'Z' || character == 'z'
                   || character == '0'
                   || character == '1'
                   || character == '2'
                   || character == '3'
                   || character == '4'
                   || character == '5'
                   || character == '6'
                   || character == '7'
                   || character == '8'
                   || character == '9'
                   || character == '_';
        }

        private static bool IsNumericChar(char character)
        {
            return character == '0'
                   || character == '1'
                   || character == '2'
                   || character == '3'
                   || character == '4'
                   || character == '5'
                   || character == '6'
                   || character == '7'
                   || character == '8'
                   || character == '9';
        }

        private bool ScanIdentifier(out string value)
        {
            value = string.Empty;
            var counter = 0;
            try
            {
                while (!IsBreakChar(ScanBuffer[ScanPointer + counter]))
                {
                    if (IsIdentifierChar(ScanBuffer[ScanPointer + counter]))
                    {
                        value += ScanBuffer[ScanPointer + counter];
                        counter++;
                    }
                    else
                    {
                        return false;
                    }
                }
                ScanPointer += counter;
                return !string.IsNullOrEmpty(value);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private bool ScanString(out string value)
        {
            // save scan pointer
            var tempScanPointer = ScanPointer;
            var tempValue = "";

            // recognize a string
            if (ScanBuffer[ScanPointer + 0] == '\''
                || ScanBuffer[ScanPointer + 0] == '"')
            {
                ScanPointer++;
                while (ScanBuffer[ScanPointer + 0] != '\''
                       && ScanBuffer[ScanPointer + 0] != '"'
                       && ScanBuffer[ScanPointer + 0] != '\0')
                {
                    tempValue += ScanBuffer[ScanPointer + 0];
                    ScanPointer++;
                }

                // check for the infamous 'run string'
                if (ScanBuffer[ScanPointer + 0] != '\0')
                {
                    // land on next char and accept if we get end string character
                    ScanPointer++;
                    value = tempValue;
                    return true;
                }
            }

            // restore scan pointer
            ScanPointer = tempScanPointer;
            value = null;
            return false;
        }

        private bool ScanInteger(out string value)
        {
            var tempScanPointer = ScanPointer;
            var tempValue = "";
            if (ScanBuffer[ScanPointer + 0] == '-'
                || IsNumericChar(ScanBuffer[ScanPointer + 0]))
            {
                // get first negative or character
                tempValue += ScanBuffer[ScanPointer + 0];
                ScanPointer++;

                // continue to scan with numeric chars
                while (IsNumericChar(ScanBuffer[ScanPointer + 0]))
                {
                    tempValue += ScanBuffer[ScanPointer + 0];
                    ScanPointer++;
                }

                // accept
                value = tempValue;
                return true;
            }

            ScanPointer = tempScanPointer;
            value = null;
            return false;
        }
    }

    internal class ScanException : Exception
    {
        public ScanException(string message) : base(message)
        {
        }
    }
}
