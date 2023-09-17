public class CTNodeValue : CTNode
{
    public readonly CToken Token;

    public CTNodeValue(CToken token) : base(CTNodeType.Value)
    {
        Token = token;
    }
}

public class CTNodeEmpty : CTNode
{
    public CTNodeEmpty() : base(CTNodeType.NULL) { }
}

public class CTNodeOperator : CTNode
{
    public readonly CToken Operator;
    public readonly CTNode Left;
    public readonly CTNode Right;

    public CTNodeOperator(CToken op, CTNode left, CTNode right) : base(CTNodeType.Operator)
    {
        Operator = op;
        Left = left;
        Right = right;
    }
    
    public CTNodeOperator(CToken op) : base(CTNodeType.Operator)
    {
        Operator = op;
        Left = null;
        Right = null;
    }
}

