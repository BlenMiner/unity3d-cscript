namespace CScript
{
    [System.Serializable]
    public struct CScriptInstruction
    {
          public readonly Opcodes Opcode;
          public readonly long Value;
          public readonly bool HasValue;
          
          public CScriptInstruction(Opcodes opcode, long value)
          {
              Opcode = opcode;
              Value = value;
              HasValue = true;
          }
          
          public CScriptInstruction(Opcodes opcode)
          {
              Opcode = opcode;
              Value = default;
              HasValue = false;
          }
    }
}