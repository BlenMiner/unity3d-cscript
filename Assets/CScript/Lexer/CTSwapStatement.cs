namespace Riten.CScript.Lexer
{
    public class CTSwapStatement : CTStatement
    {
        public readonly CTVariable Left;
        public readonly CTVariable Right;

        public CTSwapStatement(CTVariable left, CTVariable right)
        {
            Left = left;
            Right = right;
            
            AddChild(left);
            AddChild(right);
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            var left = lexer.Consume(CTokenType.WORD, "Expected variable name");
            
            lexer.Consume(CTokenType.SWAP, "Expected <=> statement");
            
            var right = lexer.Consume(CTokenType.WORD, "Expected variable name");
            
            return new CTSwapStatement(new CTVariable(left), new CTVariable(right));
        }
    }
}