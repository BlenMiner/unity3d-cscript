using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CTokenType
{
    NULL,
    EOF,
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
    LESS_THAN_OR_EQUAL,
    MORE_THAN_OR_EQUAL,
    BOOLEAN_AND,
    BOOLEAN_OR,
    BOOLEAN_NOT,
    EQUALS,
    EQUALS_EQUALS,
    REPEAT,
    SWAP,
    STRUCT,
    IF,
    ELSE
}

[Serializable]
public enum CTOptions
{
    NONE,
    UNARY,
}

[Serializable]
public class CTLexer
{
    private List<CToken> m_tokens = new ();
    
    [SerializeField]
    private List<CTError> m_errors = new ();
    
    public IReadOnlyList<CToken> Tokens => m_tokens;
    public IReadOnlyList<CTError> Errors => m_errors;
    
    private int m_parsingIndex;
    
    public int ParsingIndex => m_parsingIndex;

    public void Parse(ReadOnlySpan<char> source)
    {
        m_parsingIndex = 0;
        m_tokens.Clear();
        m_errors.Clear();

        var start = 0;
        var end = 0;

        while (end < source.Length)
        {
            var c = source[end];

            if (char.IsDigit(c))
            {
                while (end < source.Length && char.IsLetterOrDigit(source[end]))
                    end++;

                AddToken(source, CTokenType.NUMBER, start, end);
            }
            else if (char.IsLetter(c))
            {
                while (end < source.Length && char.IsLetterOrDigit(source[end]))
                    end++;

                var span = source.ToCSpan(start, end);
                AddWord(source, span, start, end);
            }
            else end = AddSingleCharTokens(source, c, start, end);

            start = end;
        }
    }

    private int AddSingleCharTokens(ReadOnlySpan<char> source, char c, int start, int end)
    {
        switch (c)
        {
            case '+': AddToken(source, CTokenType.PLUS, start, ++end); break;
            case '-': AddToken(source, CTokenType.MINUS, start, ++end); break;
            case '*': AddToken(source, CTokenType.MULTIPLY, start, ++end); break;
            case '/': AddToken(source, CTokenType.DIVIDE, start, ++end); break;
            case '(': AddToken(source, CTokenType.LEFT_PARENTHESES, start, ++end); break;
            case ')': AddToken(source, CTokenType.RIGHT_PARENTHESES, start, ++end); break;
            case '{': AddToken(source, CTokenType.LEFT_BRACE, start, ++end); break;
            case '}': AddToken(source, CTokenType.RIGHT_BRACE, start, ++end); break;
            case '&':
                if (end + 1 < source.Length && source[end + 1] == '&')
                {
                    AddToken(source, CTokenType.BOOLEAN_AND, start, end + 2);
                    end += 2;
                }
                else AddToken(source, CTokenType.BIT_AND, start, ++end);
                break;
            case '|':
                
                if (end + 1 < source.Length && source[end + 1] == '|')
                {
                    AddToken(source, CTokenType.BOOLEAN_OR, start, end + 2);
                    end += 2;
                }
                else
                {
                    AddToken(source, CTokenType.BIT_OR, start, ++end);
                    end++;
                }
                break;

            case '^': AddToken(source, CTokenType.BIT_XOR, start, ++end); break;
            case '~': AddToken(source, CTokenType.BIT_NOT, start, ++end, true); break;
            case ';': AddToken(source, CTokenType.SEMICOLON, start, ++end); break;
            case ',': AddToken(source, CTokenType.COMMA, start, ++end); break;
            case '<':
                if (end + 1 < source.Length && source[end + 1] == '<')
                {
                    AddToken(source, CTokenType.BIT_SHIFT_LEFT, start, end + 2);
                    end += 2;
                }
                else if (end + 1 < source.Length && source[end + 1] == '=' && end + 2 < source.Length && source[end + 2] == '>')
                {
                    AddToken(source, CTokenType.SWAP, start, end + 3);
                    end += 3;
                }
                else if (end + 1 < source.Length && source[end + 1] == '=')
                {
                    AddToken(source, CTokenType.LESS_THAN_OR_EQUAL, start, end + 2);
                    end += 2;
                }
                else AddToken(source, CTokenType.LESS_THAN, start, ++end);
                break;
            case '>':
                if (end + 1 < source.Length && source[end + 1] == '>')
                {
                    AddToken(source, CTokenType.BIT_SHIFT_RIGHT, start, end + 2);
                    end += 2;
                }
                else if (end + 1 < source.Length && source[end + 1] == '=')
                {
                    AddToken(source, CTokenType.MORE_THAN_OR_EQUAL, start, end + 2);
                    end += 2;
                }
                else AddToken(source, CTokenType.MORE_THAN, start, ++end);
                break;
            case '=':
                if (end + 1 < source.Length && source[end + 1] == '=')
                {
                    AddToken(source, CTokenType.EQUALS_EQUALS, start, end + 2);
                    end += 2;
                }
                else AddToken(source, CTokenType.EQUALS, start, ++end);
                break;
            case '!': AddToken(source, CTokenType.BOOLEAN_NOT, start, ++end, true); break;
            default: end++; break;
        }

        return end;
    }

    private void AddWord(ReadOnlySpan<char> source, CSpan span, int start, int end)
    {
        switch (span.Content)
        {
            case "if":     AddToken(CTokenType.IF, span); break;
            case "else":   AddToken(CTokenType.ELSE, span); break;
            case "struct": AddToken(CTokenType.STRUCT, span); break;
            case "return": AddToken(CTokenType.RETURN, span); break;
            case "repeat": AddToken(CTokenType.REPEAT, span); break;
            default: 
                AddToken(source, CTokenType.WORD, start, end); break;
        }
    }

    private void AddToken(ReadOnlySpan<char> source, CTokenType type, int start, int end, bool unary = false)
    {
        m_tokens.Add(new CToken
        {
            Type = type,
            Span = source.ToCSpan(start, end),
            Options = unary ? CTOptions.UNARY : CTOptions.NONE
        });
    }
    
    private void AddToken(CTokenType type, CSpan span)
    {
        m_tokens.Add(new CToken
        {
            Type = type,
            Span = span
        });
    }

    public int GetLine(int index)
    {
        var line = 1;
        var i = 0;

        while (i < index)
        {
            if (m_tokens[i].Span.Content == "\n")
                line++;
            i++;
        }

        return line;
    }
    
    public int GetColumn(int index)
    {
        var column = 1;
        var i = 0;

        while (i < index)
        {
            if (m_tokens[i].Span.Content == "\n")
                column = 1;
            else
                column++;
            i++;
        }

        return column;
    }

    public CToken Consume(CTokenType type, string expected)
    {
        if (m_parsingIndex >= m_tokens.Count)
            throw new CTLexerException(new CSpan(), $"Unexpected end of file, {expected}.");

        if (m_tokens[m_parsingIndex].Type != type)
            throw new CTLexerException(m_tokens[m_parsingIndex], $"{expected}, got '{m_tokens[m_parsingIndex].Span}' ({m_tokens[m_parsingIndex].Type}).");

        return m_tokens[m_parsingIndex++];
    }
    
    public int SkipUntilRightBrace()
    {
        while (m_parsingIndex < m_tokens.Count && m_tokens[m_parsingIndex].Type != CTokenType.RIGHT_BRACE)
            m_parsingIndex++;

        m_parsingIndex++;

        return m_parsingIndex;
    }

    public CToken PeekPrevious()
    {
        if (m_parsingIndex <= 0)
            throw new CTLexerException(new CSpan(), "Unexpected start of file.");
        return m_tokens[m_parsingIndex - 1];
    }
    
    public bool IsEndOfFile()
    {
        return m_parsingIndex >= m_tokens.Count;
    }

    public CToken Peek()
    {
        if (m_parsingIndex >= m_tokens.Count)
            throw new CTLexerException(new CSpan(), "Unexpected end of file.");
        return m_tokens[m_parsingIndex];
    }

    public CToken Consume()
    {
        m_parsingIndex++;
        return Peek();
    }

    public void RegisterError(CTError error)
    {
        m_errors.Add(error);
    }
    
    public bool MatchsSignature(IReadOnlyList<CTokenType> types)
    {
        if (m_parsingIndex >= m_tokens.Count)
            return false;

        if (m_tokens.Count - m_parsingIndex < types.Count)
            return false;

        for (var i = 0; i < types.Count; i++)
        {
            var type = types[i];
            
            if (type == CTokenType.NULL) continue;
            
            if (m_tokens[m_parsingIndex + i].Type != type)
                return false;
        }

        return true;
    }
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
    public static CSpan ToCSpan(this ReadOnlySpan<char> source, int start, int end)
    {
        return new CSpan(start, end, source.Slice(start, end - start).ToString());
    }
}