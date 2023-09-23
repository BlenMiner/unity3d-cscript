namespace CScript
{
    [System.Serializable]
    public struct CScriptInstruction
    {
          public readonly int OpcodeIndex;
          public readonly Opcodes Opcode;
          public readonly long Value;
          public readonly int HasValue;
          
          public CScriptInstruction(Opcodes opcode, long value)
          {
              Opcode = opcode;
              OpcodeIndex = (int)opcode;
              Value = value;
              HasValue = 1;
          }
          
          public CScriptInstruction(Opcodes opcode)
          {
              Opcode = opcode;
              OpcodeIndex = (int)opcode;
              Value = default;
              HasValue = 0;
          }
    }
}