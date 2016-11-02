namespace MonkeyCode
{
    public class Identifier : IValue
    {
        public string Name { get; set; }
        public bool Temporary { get; set; }

        public string GetValue()
        {
            return Name;
        }
    }
}