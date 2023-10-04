namespace Riten.CScript.Lexer
{
    public class CTVariable : CTNode
    {
        public CToken Identifier;
        
        public CTVariable(CToken identifier) : base(CTNodeType.Variable)
        {
            Identifier = identifier;
        }
    }
}