using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CTokenType
{
    NULL,
    RETURN,
    WORD,
    NUMBER,
    PLUS,
    MINUS,
    MULTIPLY,
    DIVIDE,
    LEFT_PARENTHESES,
    RIGHT_PARENTHESES,
    LEFT_BRACE,
    RIGHT_BRACE,
    SEMICOLON,
    COMMA,
    BIT_SHIFT_LEFT,
    BIT_SHIFT_RIGHT,
    BIT_AND,
    BIT_OR,
    BIT_XOR,
    BIT_NOT,
    LESS_THAN,
    MORE_THAN,
    BOOLEAN_AND,
    BOOLEAN_OR,
    BOOLEAN_NOT,
    EQUALS,
    EQUALS_EQUALS,
    REPEAT,
    SWAP,
    STRUCT
}

[Serializable]
public enum CTOptions
{
    NONE,
    UNARY,
}

[Serializable]
public struct CToken
{
    public CTokenType Type;
    public CTOptions Options;
    public CSpan Span;

    public override string ToString()
    {
        return Span.ToString();
    }
}

[Serializable]
public struct CSpan
{
    public int Start;
    public int End;
    public string Content;

    public override bool Equals(object obj)
    {
        if (obj is CSpan other)
            return Equals(other);
        return false;
    }

    public bool Equals(CSpan other)
    {
        return Start == other.Start && End == other.End;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End, Content);
    }

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

[Serializable]
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
            case '{':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.LEFT_BRACE,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '}':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.RIGHT_BRACE,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '&':
                if (end + 1 < source.Length && source[end + 1] == '&')
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.BOOLEAN_AND,
                        Span = source.ToCSpan(start, end + 2),
                    });

                    end += 2;
                }
                else
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.BIT_AND,
                        Span = source.ToCSpan(start, end + 1),
                    });

                    end++;
                }
                break;
            case '|':
                
                if (end + 1 < source.Length && source[end + 1] == '|')
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.BOOLEAN_OR,
                        Span = source.ToCSpan(start, end + 2),
                    });

                    end += 2;
                }
                else
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.BIT_OR,
                        Span = source.ToCSpan(start, end + 1),
                    });

                    end++;
                }
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
            
            case ',':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.COMMA,
                    Span = source.ToCSpan(start, end + 1),
                });

                end++;
                break;
            case '<':
                if (end + 1 < source.Length && source[end + 1] == '<')
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.BIT_SHIFT_LEFT,
                        Span = source.ToCSpan(start, end + 2),
                    });

                    end += 2;
                }
                else if (end + 1 < source.Length && source[end + 1] == '=' && end + 2 < source.Length && source[end + 2] == '>')
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.SWAP,
                        Span = source.ToCSpan(start, end + 3),
                    });

                    end += 3;
                }
                else
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.LESS_THAN,
                        Span = source.ToCSpan(start, end + 1),
                    });

                    end++;
                }
                break;
            case '>':
                if (end + 1 < source.Length && source[end + 1] == '>')
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.BIT_SHIFT_RIGHT,
                        Span = source.ToCSpan(start, end + 2),
                    });

                    end += 2;
                }
                else
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.MORE_THAN,
                        Span = source.ToCSpan(start, end + 1),
                    });

                    end++;
                }
                break;
            case '=':
                if (end + 1 < source.Length && source[end + 1] == '=')
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.EQUALS_EQUALS,
                        Span = source.ToCSpan(start, end + 2),
                    });

                    end += 2;
                }
                else
                {
                    m_tokens.Add(new CToken
                    {
                        Type = CTokenType.EQUALS,
                        Span = source.ToCSpan(start, end + 1),
                    });

                    end++;
                }
                break;
            case '!':
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.BOOLEAN_NOT,
                    Options = CTOptions.UNARY,
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
            case "struct":
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.STRUCT,
                    Span = span,
                });
                break;
            case "return":
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.RETURN,
                    Span = span,
                });
                break;
            case "repeat":
                m_tokens.Add(new CToken
                {
                    Type = CTokenType.REPEAT,
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
