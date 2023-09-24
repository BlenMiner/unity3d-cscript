namespace CScript
{
    [System.Serializable]
    public struct CScriptInstruction
    {
          public readonly int OpcodeIndex;
          public readonly Opcodes Opcode;
          public readonly long Value;
          
          public readonly int HasValue;
          public readonly int HasValueInverse;
          
          public CScriptInstruction(Opcodes opcode, long value)
          {
              Opcode = opcode;
              OpcodeIndex = (int)opcode;
              Value = value;
              HasValue = -1; 
              HasValueInverse = 0;
          }
          
          public CScriptInstruction(Opcodes opcode)
          {
              Opcode = opcode;
              OpcodeIndex = (int)opcode;
              Value = default;
              HasValueInverse = -1;
              HasValue = 0;
          }
    }
}