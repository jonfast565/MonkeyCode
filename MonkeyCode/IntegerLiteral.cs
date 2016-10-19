namespace MonkeyCode
{
    internal class IntegerLiteral : IValue
    {
        public int Value { get; set; }

        public string GetValue()
        {
            return Value.ToString();
        }
    }
}