using System;
using System.Diagnostics;

namespace Shine.Http
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc7230#section-3.2
    /// obs-fold and obs-text omitted
    /// </summary>
    public class HttpHeaderField
    {
        // fieldnames are case-insensitive
        public readonly string Name;
        public string Value { get; private set; }

        public HttpHeaderField(string name, string value)
        {
            Debug.Assert(HttpValidationUtilities.IsValidToken(name), $"Http field {nameof(name)} is not valid");
            Debug.Assert(HttpValidationUtilities.IsValidHeaderFieldValue(value), $"Http field {nameof(value)} is not valid");

            Name = name;
            Value = value;
        }

        public void Fold(HttpHeaderField field)
        {
            if (Name.Equals(field.Name, StringComparison.OrdinalIgnoreCase))
            {
                Value = String.Concat(Value, ", ", field.Value);
            }
        }
    }
}
