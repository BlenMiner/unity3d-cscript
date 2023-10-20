namespace Riten.CScript.Lexer
{
    public class CTVariable : CTTypedNode
    {
        public CToken Identifier;
        
        public CTVariable(CToken identifier)
        {
            Identifier = identifier;
        }
    }
}