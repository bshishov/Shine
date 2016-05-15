using System;

namespace Shine.Middleware.Session
{
    public interface ISessionFactory
    {
        ISessionContext Create(string key, DateTime expires);
    }

    public class DefaultSessionFactory : ISessionFactory
    {
        public ISessionContext Create(string key, DateTime expires)
        {
            return new SessionContext(key, expires);
        }
    }
}