namespace Riten.CScript.Lexer
{
    public class CTConstValue : CTNode
    {
        public readonly CToken Token;

        public CTConstValue(CToken token) : base(CTNodeType.Value)
        {
            Token = token;
        }
    }

    public class CTNodeEmpty : CTNode
    {
        public CTNodeEmpty() : base(CTNodeType.NULL)
        {
        }
    }

    public class CTOperator : CTNode
    {
        public readonly CToken Operator;
        public readonly CTNode Left;
        public readonly CTNode Right;

        public CTOperator(CToken op, CTNode left, CTNode right) : base(CTNodeType.Operator)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public CTOperator(CToken op) : base(CTNodeType.Operator)
        {
            Operator = op;
            Left = null;
            Right = null;
        }
    }
}