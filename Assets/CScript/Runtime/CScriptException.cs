using System;

[System.Serializable]
public class CScriptException : Exception
{
    public CToken Token;

    public string Hint;

    public CScriptException(CToken token, string message, string hint = default) : base(message)
    {
        Token = token;
        Hint = hint;
    }

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Hint) ? $"({Token.Span.Start}-{Token.Span.End}): {Message}" : $"'{Token.Span}': {Message} ({Hint})";
    }
}
