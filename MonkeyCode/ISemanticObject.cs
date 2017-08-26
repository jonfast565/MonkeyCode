using System.Collections.Generic;

namespace MonkeyCode
{
    public interface ISemanticObject
    {
        List<Instruction> AppendInstructions(List<Instruction> instructionList);
    }
}