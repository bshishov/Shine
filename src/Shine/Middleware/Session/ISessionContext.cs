using System;
using Shine.Utilities;

namespace Shine.Middleware.Session
{
    public interface ISessionContext
    {
        bool CookieExist { get; set; }
        string Key { get; }
        DateTime Expires { get; }
        string Serialize();
        void Deserialize(string input);
        event Action<Context, string, object> ContextChanged;
    }
}