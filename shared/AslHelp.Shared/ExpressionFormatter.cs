using System;
using System.Reflection;
using System.Text;

namespace AslHelp.Shared;

public static class PropertyExpressionFormatter
{
    public static string Format<T>(string format, T value, BindingFlags flags, IFormatProvider? formatProvider = null)
    {
        ThrowHelper.ThrowIfNull(format);

        StringBuilder builder = new(format.Length);
        ReadOnlySpan<char> remaining = format.AsSpan();

        while (true)
        {
            int start = remaining.IndexOfAny('{', '}');
            if (start == -1)
            {
                // No more tokens.

                builder.Append(remaining.ToString());

                return builder.ToString();
            }

            builder.Append(remaining[..start].ToString());

            char startToken = remaining[start];
            if (start + 1 < remaining.Length
                && remaining[start + 1] == startToken)
            {
                // This is an escaped brace.

                builder.Append(startToken);
                remaining = remaining[(start + 2)..];

                continue;
            }

            if (startToken == '}')
            {
                ThrowHelper.ThrowFormatException("Closing brace has no matching opening brace.");
            }

            var content = remaining[(start + 1)..];

            int end = content.IndexOfAny('{', '}');
            char endToken = content[end];

            if (end == -1
                || content[end] != '}')
            {
                ThrowHelper.ThrowFormatException("Opening brace has no matching closing brace.");
            }

            content = content[..end];

            if (content.IsEmpty)
            {
                ThrowHelper.ThrowFormatException("Empty token.");
            }

            int formatDelim = content.IndexOf(':');
            string? valueFormat;
            if (formatDelim == -1)
            {
                valueFormat = null;
            }
            else
            {
                valueFormat = content[(formatDelim + 1)..].ToString();
                content = content[..formatDelim];
            }

            PropertyInfo? pInfo = typeof(T).GetProperty(content.ToString(), flags);
            if (pInfo is null)
            {
                ThrowHelper.ThrowFormatException($"Property '{content.ToString()}' not found on type '{typeof(T).FullName}'.");
            }

            object? pValue = pInfo.GetValue(value);
            if (pValue is not null)
            {
                if (valueFormat is not null && pValue is IFormattable formattable)
                {
                    builder.Append(formattable.ToString(valueFormat, formatProvider));
                }
                else
                {
                    builder.Append(pValue.ToString());
                }
            }

            remaining = remaining[(start + end + 2)..];
        }
    }
}
