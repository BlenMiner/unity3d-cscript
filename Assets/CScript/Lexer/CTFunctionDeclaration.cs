using System.Collections.Generic;

public class CTArgumentDeclaration : CTNode
{
    public readonly CToken ArgumentType;
    public readonly CToken ArgumentName;
    
    public CTArgumentDeclaration(CToken type, CToken name) : base(CTNodeType.ArgumentDeclaration)
    {
        ArgumentType = type;
        ArgumentName = name;
    }
}

public class CTArgumentsDeclaration : CTNode
{
    public readonly CToken StartParenthesis;
    public readonly IList<CTArgumentDeclaration> Arguments;
    public readonly CToken EndParenthesis;
    
    public CTArgumentsDeclaration(CToken startParenthesis, IList<CTArgumentDeclaration> args, CToken endParenthesis) 
        : base(CTNodeType.ArgumentsDeclaration)
    {
        StartParenthesis = startParenthesis;
        Arguments = args;
        EndParenthesis = endParenthesis;
    }
    
    public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
    {
        var arguments = new List<CTArgumentDeclaration>();
        
        bool expectingIdentifier = false;
        bool expectingComma = false;
        CToken type = default;
        
        var startParenthesis = tokens[i++];
        
        if (startParenthesis.Type != CTokenType.LEFT_PARENTHESES)
            throw new CTLexerException(tokens[i], $"Expected left parenthesis, got '{startParenthesis.Span}'.");
        
        while (i < tokens.Count)
        {
            var token = tokens[i];

            if (token.Type == CTokenType.RIGHT_PARENTHESES)
            {
                var last = tokens[i - 1];
                
                if (last.Type == CTokenType.COMMA)
                {
                    throw new CTLexerException(last,
                        $"Unexpected token '{last.Span}' in argument declaration.",
                        "Commans should be placed between arguments only."
                    );
                }
                
                if (expectingIdentifier)
                {
                    throw  new CTLexerException(token,
                        $"Unexpected token '{token.Span}' in argument declaration.",
                        "Expected identifier."
                    );
                }
                break;
            }

            switch (token.Type)
            {
                case CTokenType.COMMA:
                {
                    if (expectingIdentifier)
                    {
                        throw new CTLexerException(token,
                            $"Unexpected token '{token.Span}' in argument declaration.",
                            "Expected identifier."
                        );
                    }
                    
                    if (!expectingComma)
                    {
                        throw new CTLexerException(token,
                            $"Badly placed comma '{token.Span}' in argument declaration.",
                            "Commans should be placed between arguments only."
                        );    
                    }

                    expectingComma = false;
                    break;
                }
                case CTokenType.WORD:
                {
                    if (expectingComma)
                    {
                        throw new CTLexerException(token,
                            $"Unexpected token '{token.Span}' in argument declaration.",
                            "You need to separate arguments with a comma."
                        );
                    }
                    
                    if (!expectingIdentifier)
                    {
                        type = token;
                        expectingIdentifier = true;
                    }
                    else
                    {
                        arguments.Add(new CTArgumentDeclaration(type, token));
                        expectingComma = true;
                        expectingIdentifier = false;
                    }
                    break;
                }
                default:
                    throw new CTLexerException(token, $"Invalid token '{token.Span}' in argument declaration.");
            }
            
            i++;
        }
        
        if (expectingIdentifier)
            throw new CTLexerException(default, "Missing argument identifier.");
        
        if (i >= tokens.Count)
            throw new CTLexerException(default, "Expected right parenthesis, got end of file.");
        
        var endParentheis = tokens[i++];
        
        return new CTNodeResponse(new CTArgumentsDeclaration(startParenthesis, arguments, endParentheis), i);
    }
}

public class CTFunctionDeclaration : CTNode
{
    public CToken FunctionType;
    public CToken FunctionName;
    public CTArgumentsDeclaration Arguments;
    
    public CTFunctionDeclaration(CToken type, CToken name, CTArgumentsDeclaration args) : base(CTNodeType.FunctionDeclaration) { }

    public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
    {
        var functionType = tokens[i++];
        
        if (functionType.Type != CTokenType.WORD)
            throw new CTLexerException(functionType, $"Expected function type, got '{functionType.Span}'.");

        if (i >= tokens.Count)
            throw new CTLexerException(functionType, "Expected function name, got end of file.");
                
        var functionName = tokens[i++];
        
        if (functionName.Type != CTokenType.WORD)
            throw new CTLexerException(functionName, $"Expected function name, got '{functionName.Span}'.");

        if (i >= tokens.Count)
            throw new CTLexerException(functionName, "Expected function body, got end of file.");

        var arguments = CTArgumentsDeclaration.Parse(tokens, i);
        
        return new CTNodeResponse(new CTFunctionDeclaration(
            functionType, 
            functionName, 
            (CTArgumentsDeclaration)arguments.Node), 
            arguments.Index
        );
    }
}
