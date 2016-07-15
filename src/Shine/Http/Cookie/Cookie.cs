using System;
using System.Globalization;
using System.Text;

namespace Shine.Http.Cookie
{
    public class Cookie : ICookie
    {
        public Cookie(string name, string value, string domain = null, bool httpOnly = false, string path = "/", DateTime? expires = null)
        {
            Name = name;
            Value = value;
            Path = path;
            Expires = expires;
            HttpOnly = httpOnly;
            Domain = domain;
        }

        public string Name { get; }
        public string Value { get; }
        public string Path { get; set; }
        public string Domain { get; set; }
        public DateTime? Expires { get; set; }
        public bool HttpOnly { get; }
        public bool Secure { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(50);
            sb.AppendFormat("{0}={1}; path={2}", Name, Value, Path ?? "/");

            if (Expires != null)
            {
                sb.Append("; expires=");
                sb.Append(Expires.Value.ToUniversalTime().ToString("R", DateTimeFormatInfo.InvariantInfo));
            }

            if (Domain != null)
            {
                sb.Append("; domain=");
                sb.Append(Domain);
            }

            if (Secure)
            {
                sb.Append("; Secure");
            }

            if (HttpOnly)
            {
                sb.Append("; HttpOnly");
            }

            return sb.ToString();
        }
    }
}
