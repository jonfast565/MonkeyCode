namespace MonkeyCode
{
    public class Instruction
    {
        public InstructionOpcode Opcode { get; set; }
        public IValue Value1 { get; set; }
        public IValue Value2 { get; set; }
        public Identifier Target { get; set; }
        public Identifier Source { get; set; }
    }
}