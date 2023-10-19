using System;

public enum CTExceptionType
{
    Lexer,
    Parser,
    Compiler
}

public enum CTExceptionLevel
{
    Hint,
    Warning,
    Error
}

[Serializable]
public struct CTError
{
    public CTExceptionType Type;
    public CTExceptionLevel Level;
    
    public CSpan Span;
    public string Message;
    public string Details;
    
    public CTError(CSpan span, string message, string details = default)
    {
        Message = message;
        Details = details;
        Span = span;
        Level = CTExceptionLevel.Error;
        Type = CTExceptionType.Lexer;
    }
    
    public CTError(CToken span, string message, string details = default) : this(span.Span, message, details)
    {
    }
    
    public override string ToString()
    {
        var token = Span.Content ?? "EOF";
        
        return string.IsNullOrWhiteSpace(Details) ? $"({token}:{Span.Start}): {Message}" : $"'{token}': {Message} ({Message})";
    } 
}

[Serializable]
public class CTLexerException : Exception
{
    public CTError Error;

    public CTLexerException(CTError error) : base(error.Message)
    {
        Error = error;
    }
    
    public CTLexerException(CToken token, string message, string details = default) : base(message)
    {
        Error = new CTError(token.Span, message, details);
    }
    
    public CTLexerException(CSpan span, string message, string details = default) : base(message)
    {
        Error = new CTError(span, message, details);
    }

    public override string ToString() => Error.ToString();
}
