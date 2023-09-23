namespace CScript
{
    public sealed class CScriptProgram
    {
        private readonly CScriptCompiled m_compiledScript;
        private readonly CScriptStack m_stack;
        
        public CScriptStack Stack => m_stack;
        
        public CScriptProgram(CScriptCompiled compiledScript, CScriptStack stack)
        {
            CScriptOpcodeExtensions.Init();
            m_compiledScript = compiledScript;
            m_stack = stack;
        }
        
        public void Step()
        {
            var instruction = m_compiledScript.Instructions[m_stack.IP++];
            
            if (instruction.HasValue) m_stack.R0 = instruction.Value;
            
            instruction.Opcode.Execute(m_stack);
        }
        
        public void Run()
        {
            while (m_stack.IP < m_compiledScript.Instructions.Count)
                Step();
        }
    }
}