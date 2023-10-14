using System.Collections.Generic;

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
    FieldDeclaration,
    FunctionCall,
    FunctionArgs,
    IfStatement
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

    protected static int ConsumeToken(CTokenType token, out CToken value, IReadOnlyList<CToken> tokens, int i, string error)
    {
        int newIdx = ConsumeToken(token, tokens, i, error);
        
        value = tokens[i];
        
        return newIdx;
    }
    
    protected static int ConsumeToken(CTokenType token, IReadOnlyList<CToken> tokens, int i, string error)
    {
        if (i >= tokens.Count)
            throw new CTLexerException(default, $"Unexpected end of file, {error}");

        if (tokens[i].Type != token)
            throw new CTLexerException(tokens[i], error);

        return i + 1;
    }
}
