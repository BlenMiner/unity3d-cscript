using System.Collections.Generic;
using Riten.CScript.Lexer;

public class CTArgumentDeclaration : CTNode
{
    public readonly CToken ArgumentType;
    public readonly CToken ArgumentName;
    
    public CTArgumentDeclaration(CToken type, CToken name)
    {
        ArgumentType = type;
        ArgumentName = name;
    }
}

public class CTArgumentsDeclaration : CTStatement
{
    public readonly CToken StartParenthesis;
    public readonly IList<CTArgumentDeclaration> Values;
    public readonly CToken EndParenthesis;

    private CTArgumentsDeclaration(CToken startParenthesis, IList<CTArgumentDeclaration> args, CToken endParenthesis)
    {
        StartParenthesis = startParenthesis;
        Values = args;
        EndParenthesis = endParenthesis;
    }
    
    public static CTNode Parse(CTLexer lexer)
    {
        var arguments = new List<CTArgumentDeclaration>();
        
        bool expectingIdentifier = false;
        bool expectingComma = false;
        CToken type = default;
        
        var startParenthesis = lexer.Consume(CTokenType.LEFT_PARENTHESES, "Expected left parenthesis for argument declaration.");
        
        while (true)
        {
            var token = lexer.Peek();

            if (token.Type == CTokenType.RIGHT_PARENTHESES)
            {
                var last = lexer.PeekPrevious();
                
                if (last.Type == CTokenType.COMMA)
                {
                    throw new CTLexerException(last,
                        $"Unexpected token '{last.Span}' in argument declaration",
                        "Commans should be placed between arguments only"
                    );
                }
                
                if (expectingIdentifier)
                {
                    throw  new CTLexerException(token,
                        $"Unexpected token '{token.Span}' in argument declaration",
                        "Expected identifier"
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
                            $"Unexpected token '{token.Span}' in argument declaration",
                            "Expected identifier"
                        );
                    }
                    
                    if (!expectingComma)
                    {
                        throw new CTLexerException(token,
                            $"Badly placed comma '{token.Span}' in argument declaration",
                            "Commans should be placed between arguments only"
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
                            $"Unexpected token '{token.Span}' in argument declaration",
                            "You need to separate arguments with a comma"
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
                    throw new CTLexerException(token, $"Invalid token '{token.Span}' in argument declaration");
            }

            lexer.Consume();
        }
        
        if (expectingIdentifier)
            throw new CTLexerException(new CSpan(), "Missing argument identifier.");
        
        var endParentheis = lexer.Consume(CTokenType.RIGHT_PARENTHESES, "Expected right parenthesis for argument declaration");
        
        return new CTArgumentsDeclaration(startParenthesis, arguments, endParentheis);
    }
}

public class CTFunction : CTDefinition
{
    public CToken ReturnType;
    public CToken FunctionName;
    public readonly CTArgumentsDeclaration Arguments;
    public readonly CTBlockStatement BlockStatement;

    public CTFunction(CToken type, CToken name, CTArgumentsDeclaration args, CTBlockStatement blockStatement)
    {
        ReturnType = type;
        FunctionName = name;
        Arguments = args;
        BlockStatement = blockStatement;
        
        AddChild(blockStatement);
    }

    public static CTNode Parse(CTLexer lexer)
    {
        var functionType = lexer.Consume(CTokenType.WORD, "Expected function type for function declaration");
        var functionName = lexer.Consume(CTokenType.WORD, "Expected function name for function declaration");
        
        var arguments = CTArgumentsDeclaration.Parse(lexer);
        var block = CTBlockStatement.Parse(lexer);
        
        return new CTFunction(
            functionType, 
            functionName, 
            (CTArgumentsDeclaration)arguments, 
            (CTBlockStatement)block
        );
    }
}
