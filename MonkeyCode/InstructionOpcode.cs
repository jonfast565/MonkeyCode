namespace MonkeyCode
{
    public enum InstructionOpcode
    {
        None = 0,
        Move,
        Add,
        Subtract,
        Multiply,
        Divide,
        Allocate,
        Label,
        Compare,
        Jump,
        Print
    }
}