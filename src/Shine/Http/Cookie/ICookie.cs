using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Http.Cookie
{
    public interface ICookie
    {
        string Name { get; }
        string Value { get; }
        string Path { get; }
        string Domain { get; }
        DateTime? Expires { get; }
        bool HttpOnly { get; }
        bool Secure { get; }
    }
}
