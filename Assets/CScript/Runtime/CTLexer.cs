using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CTokenType
{
    NULL,
    WORD,
    NUMBER,
    PLUS,
    MINUS,
    MULTIPLY,
    DIVIDE,
    LEFT_PARENTHESES,
    RIGHT_PARENTHESES,
    SEMICOLON,
    BIT_SHIFT_LEFT,
    BIT_SHIFT_RIGHT,
    BIT_AND,
    BIT_OR,
    BIT_XOR,
    BIT_NOT
}

[System.Serializable]
public enum CTOptions
{
    NONE,
    UNARY,
}

[System.Serializable]
public struct CToken
{
    public CTokenType Type;
    public CTOptions Options;
    public CSpan Span;
}

[System.Serializable]
public struct CSpan
{
    public int Start;
    public int End;
    public string Content;
    
    public CSpan(int start, int end, string content)
    {
        Start = start;
        End = end;
        Content = content;
    }

    public override string ToString()
    {
        return Content;
    }
}

public static class CSpanUtils
{
    public static CSpan ToCSpan(this string source, int start, int end)
    {
        return new CSpan(start, end, source.Substring(start, end - start));
    }
}

[System.Serializable]
public class CTLexer
{
    [SerializeField] List<CToken> m_tokens = new ();
    
    public IReadOnlyList<CToken> Tokens => m_tokens;

    public void Parse(string source)
    {
        m_tokens.Clear();
        
        var start = 0;
        var end = 0;

        while (end < source.Length)
        {
            var c = source[end];

            if (char.IsDigit(c))
            {
                while (end < source.Length && char.IsLetterOrDigit(source[end]))
                    end++;

                m_tokens.Add(new CToken
                {
                    Type = CTokenType.NUMBER,
                    Span = source.ToCSpan(start, end),
                });
            }
            else if (char.IsLetter(c))
            {
                while (end < source.Length && char.IsLetterOrDigit(source[end]))
                    end++;

                var span = source.ToCSpan(start, end);
                AddWord(source, span, start, end);
            }
            else
                end = AddSingleCharTokens(source, c, start, end);

            start = end;
        }
    }

    private int AddSingleCharTokens(string source, char c, int start, int end)
    {
        switch (c)
        {
            case '+':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.PLUS,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '-':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.MINUS,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '*':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.MULTIPLY,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '/':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.DIVIDE,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '(':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.LEFT_PARENTHESES,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case ')':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.RIGHT_PARENTHESES,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '&':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BIT_AND,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '|':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BIT_OR,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;

            case '^':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BIT_XOR,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '~':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BIT_NOT,
                    Options = CTOptions.UNARY,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case ';':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.SEMICOLON,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            default:
                end++;
                break;
        }

        return end;
    }

    private void AddWord(string source, CSpan span, int start, int end)
    {
        switch (span.Content)
        {
            case "<<":
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BIT_SHIFT_LEFT,
                    Span = span,
                });
                break;
            case ">>":
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BIT_SHIFT_RIGHT,
                    Span = span,
                });
                break;
            default:
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.WORD,
                    Span = source.ToCSpan(start, end),
                });
                break;
        }
    }
}
