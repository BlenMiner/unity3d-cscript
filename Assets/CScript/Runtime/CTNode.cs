[System.Serializable]
public enum CTNodeType
{
    NULL,
    Root,
    Expression,
    Operator,
    Value,
    FunctionDeclaration,
    ArgumentsDeclaration,
    ArgumentDeclaration
}

public abstract class CTNode
{
    public readonly CTNodeType NodeType;

    protected CTNode(CTNodeType type)
    {
        NodeType = type;
    }
}
