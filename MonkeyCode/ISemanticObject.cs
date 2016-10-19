using System.Collections.Generic;

namespace MonkeyCode
{
    internal interface ISemanticObject
    {
        List<Instruction> AppendInstructions(List<Instruction> instructionList);
    }
}