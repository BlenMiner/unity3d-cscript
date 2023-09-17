[System.Serializable]
public enum CTNodeType
{
    NULL,
    Root,
    Expression,
    Operator,
    OperatorOrValue,
    Value
}

public abstract class CTNode
{
    public readonly CTNodeType NodeType;

    protected CTNode(CTNodeType type)
    {
        NodeType = type;
    }
}
