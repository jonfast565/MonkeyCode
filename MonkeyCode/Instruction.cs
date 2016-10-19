namespace MonkeyCode
{
    internal class Instruction
    {
        public InstructionOpcode Opcode { get; set; }
        public IValue Value1 { get; set; }
        public IValue Value2 { get; set; }
        public Identifier Result { get; set; }
    }
}