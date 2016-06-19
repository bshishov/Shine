using System;
using System.Collections.Generic;
using System.Net;
using Shine.Responses;
using Shine.Utilities;

namespace Shine.Middleware.Session
{
    /// <summary>
    ///     Middleware that provides basic identification of incoming requests giving each request a session object
    /// </summary>
    public class SessionMiddleware : IMiddleware
    {
        private readonly ISessionStorage _storage;
        private readonly ISessionFactory _factory;
        private readonly string _secret;
        private const string SessionKeyCookieName = "SID";
        private readonly TimeSpan _sessionLifeTime; // TODO: To settings?
        private readonly Dictionary<string, ISessionContext> _sessions = new Dictionary<string, ISessionContext>();

        public SessionMiddleware(string secret, ISessionStorage storage, ISessionFactory factory, TimeSpan sessionLifeTime)
        {
            _storage = storage;
            _factory = factory;
            _secret = secret;
            _sessionLifeTime = sessionLifeTime;
        }

        public SessionMiddleware(string secret, ISessionStorage storage, TimeSpan sessionLifeTime) : this(secret, storage, new DefaultSessionFactory(), sessionLifeTime) { }

        public SessionMiddleware(string secret, TimeSpan sessionLifeTime) : this(secret, new MemorySessionStorage(), new DefaultSessionFactory(), sessionLifeTime) { }

        /// <summary>
        ///     Before request
        /// </summary>
        /// <param name="request">Input request</param>
        public void Handle(IRequest request)
        {
            string key = null;

            if (request.Cookies != null)
                key = request.Cookies[SessionKeyCookieName];

            if (String.IsNullOrEmpty(key))
            {
                InitNewSession(request, Guid.NewGuid().ToString());
            }
            else
            {
                if (_sessions.ContainsKey(key))
                {
                    // Set existing session
                    request.Session = _sessions[key];
                    request.Session.CookieExist = true;
                }
                else
                {
                    var existingSession = LoadSession(key);
                    if (existingSession != null)
                    {
                        RegisterSession(request, existingSession);
                    }
                    else
                    {
                        // Create new session with provided key
                        // TODO: Remove ? throw "bad session key"? 
                        InitNewSession(request, key, true);
                    }
                }
            }
        }

        /// <summary>
        ///     After request
        /// </summary>
        /// <param name="request">Input request</param>
        /// <param name="response">Output response</param>
        public void Handle(IRequest request, Response response)
        {
            if (request.Session != null && !request.Session.CookieExist)
            {
                var httpresponse = response as HttpResponse;
                if (httpresponse == null)
                    return;
                httpresponse.Cookies.Add(new Cookie(SessionKeyCookieName, request.Session.Key, "/")
                {
                    Expires = request.Session.Expires,
                    HttpOnly = true
                });
            }
        }

        /// <summary>
        ///     Initialize new session and adds it to the dictionary
        /// </summary>
        /// <param name="request">Input request</param>
        /// <param name="key">SessionContext key (GUID or existing session key)</param>
        /// <param name="cookieExists">Defines wheter cookie set on client or not</param>
        private void InitNewSession(IRequest request, string key, bool cookieExists = false)
        {
            var session = _factory.Create(key, DateTime.Now + _sessionLifeTime);
            session.CookieExist = cookieExists;
            session.ContextChanged += SessionOnContextChanged;
            SaveSession(session);
            RegisterSession(request, session);
        }

        private void SessionOnContextChanged(Context context, string s, object arg3)
        {
            var session = context as SessionContext;
            if(session == null)
                throw new InvalidOperationException("SessionContext expected");

           
                SaveSession(session);
        }

        private void RegisterSession(IRequest request, ISessionContext session)
        {
            _sessions.Add(session.Key, session);
            request.Session = session;
        }

        private void SaveSession(ISessionContext sessionContext)
        {
            var encryptedData = StringCipher.Encrypt(sessionContext.Serialize(), _secret);

            _storage.Save(new SessionInfo()
            {
                Key = sessionContext.Key,
                Expires = sessionContext.Expires,
                Data = encryptedData
            });
        }

        private ISessionContext LoadSession(string key)
        {
            var existingSession = _storage.Load(key);
            if (existingSession != null)
            {
                // If session expired
                if (DateTime.Now > existingSession.Expires)
                {
                    _storage.Delete(existingSession);
                    return null;
                }
                var sessionContext = _factory.Create(existingSession.Key, existingSession.Expires);
                sessionContext.ContextChanged += SessionOnContextChanged;

                var data = StringCipher.Decrypt(existingSession.Data, _secret);
                sessionContext.Deserialize(data);
                return sessionContext;
            }
            return null;
        }
    }
}