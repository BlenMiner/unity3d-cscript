﻿using System;

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
        var token = Token.Span.Content ?? "EOF";
        
        return string.IsNullOrWhiteSpace(Hint) ? $"({token}): {Message}" : $"'{token}': {Message} ({Hint})";
    }
}
