using System;

[Serializable]
public class CTLexerException : Exception
{
    public CToken Token;

    public string Hint;

    public CTLexerException(CToken token, string message, string hint = default) : base(message)
    {
        Token = token;
        Hint = hint;
    }

    public override string ToString()
    {
        var token = Token.Span.Content ?? "EOF";
        
        return string.IsNullOrWhiteSpace(Hint) ? $"({token}:{Token.Span.Start}): {Message}" : $"'{token}': {Message} ({Hint})";
    }
}
