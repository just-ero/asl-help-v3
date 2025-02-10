using System;
using System.Text;

namespace AslHelp.Shared;

public static class ExpressionFormatter
{
    public static string Format(string format, object tokenValues, FormatOptions options = default)
    {
        return Format(format, tokenValues, null, options);
    }

    public static string Format(string format, object value, IFormatProvider? formatProvider, FormatOptions options = default)
    {
        ThrowHelper.ThrowIfNull(format);

        StringBuilder sb = new(format.Length);

        bool nonPublic = options.HasFlag(FormatOptions.NonPublic);
        bool allowMissingKeys = options.HasFlag(FormatOptions.AllowMissingKeys);

        ReadOnlySpan<char> remaining = format.AsSpan();

        while (true)
        {
            int start = remaining.IndexOfAny('{', '}');
            if (start == -1)
            {
                sb.Append(remaining.ToString());
                return sb.ToString();
            }

            sb.Append(remaining[..start].ToString());

            char startToken = remaining[start];

            if (start + 1 < remaining.Length
                && remaining[start + 1] == startToken)
            {
                sb.Append(startToken);
                remaining = remaining[(start + 2)..];
                continue;
            }

            if (startToken == '}')
            {
                ThrowHelper.ThrowFormatException("Closing brace has no matching opening brace.");
            }

            var content = remaining[(start + 1)..];
            int end = content.IndexOfAny('{', '}');

            if (end == -1
                || content[end] != '}')
            {
                ThrowHelper.ThrowFormatException("Opening brace has no matching closing brace.");
            }

            content = content[..end];

            if (content.Length == 0)
            {
                ThrowHelper.ThrowFormatException("Empty token.");
            }

            var
        }
    }
}

file ref struct Token
{
    private readonly ReadOnlySpan<char> _content;

    public TokenKey Key { get; }
    public ReadOnlySpan<char> Format { get; }
    public readonly bool HasSubKey => !_content.IsEmpty;
    public bool HasFormat { get; }

    public Token(ReadOnlySpan<char> content)
    {
        int delimiter = content.IndexOf(':');

        if (delimiter == -1)
        {
            _content = content;
            Format = [];
            HasFormat = false;
        }
        else
        {
            _content = content[..delimiter];
            Format = content[(delimiter + 1)..];
            HasFormat = true;
        }

        Key = new TokenKey(_content);
    }

    public void ProcessSubKey()
    {
        Key = ProcessKey(ref _content);
    }

    private static TokenKey ProcessKey(ref ReadOnlySpan<char> content)
    {
        if (content.IsEmpty)
        {
            ThrowHelper.ThrowFormatException("Empty token.");
        }

        int subkeyDelimiter = content.IndexOf('.');

        TokenKey key;

        if (subkeyDelimiter == -1)
        {
            key = new(content);
        }
        else
        {
            key = new(content[..subkeyDelimiter]);
            content = content[(subkeyDelimiter + 1)..];
        }

        return key;
    }
}

file ref struct TokenKey
{
    public TokenKey(ReadOnlySpan<char> content)
    {
        int delimiter = content.IndexOf('?');

        if (delimiter == -1)
        {
            Name = content;
            Default = [];
            HasDefault = false;
        }
        else
        {
            Name = content[..delimiter];
            Default = content[(delimiter + 1)..];
            HasDefault = true;
        }

        if (Name.IsEmpty)
        {
            ThrowHelper.ThrowFormatException("Empty token.");
        }
    }

    public ReadOnlySpan<char> Name { get; }
    public ReadOnlySpan<char> Default { get; }
    public bool HasDefault { get; }
}

public enum FormatOptions
{

}
