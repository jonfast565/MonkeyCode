namespace MonkeyCode
{
    public class Token
    {
        public Token(TokenType type)
        {
            Type = type;
        }

        public Token(TokenType type, string lexeme)
        {
            Type = type;
            Lexeme = lexeme;
        }

        public string Lexeme { get; private set; }
        public TokenType Type { get; private set; }
    }
}