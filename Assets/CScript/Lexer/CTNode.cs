[System.Serializable]
public enum CTNodeType
{
    NULL,
    Root,
    Expression,
    DeclareStatement,
    Operator,
    Value,
    Variable,
    FunctionDeclaration,
    ArgumentsDeclaration,
    ArgumentDeclaration,
    Type,
    Block,
    AssignStatement,
    ReturnStatement,
    RepeatStatement,
    SWAPStatement,
    StructDefinition,
    FieldDeclaration
}

public abstract class CTStatement : CTNode
{
    protected CTStatement(CTNodeType type) : base(type)
    {
    }
}

public abstract class CTNode
{
    public readonly CTNodeType NodeType;

    protected CTNode(CTNodeType type)
    {
        NodeType = type;
    }
}
