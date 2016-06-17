using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Shine.Responses;

namespace Shine.Middleware.CSRF
{
    public class CsrfMiddleware : IMiddleware
    {
        public const string CsrfKey = "csrf_token";
        private readonly string[] _trustedMethods = { "GET", "TRACE", "HEAD", "OPTIONS" };
        private readonly SHA256Managed _crypt;
        private readonly string _salt;
        private readonly bool _cookieCheck;

        public CsrfMiddleware(string salt, bool useCookiesToCheck = true)
        {
            _cookieCheck = useCookiesToCheck;
            _crypt = new SHA256Managed();
            _salt = salt;
        }

        public void Handle(IRequest request)
        {
            var isTrustedMethod = _trustedMethods.Contains(request.Method);

            // if method requires CSRF token verification
            if (!isTrustedMethod)
            {
                var storedToken = request.Session?.Get<string>(CsrfKey);
                if(storedToken == null)
                    throw new CsrfVerificationException();

                if (_cookieCheck)
                {
                    var cookieToken = request.Cookies[CsrfKey];
                    if(!string.IsNullOrEmpty(cookieToken))
                        if(cookieToken.Equals(storedToken))
                            // CSRF token verification passed
                            return;
                }

                var postToken = request.PostArgs[CsrfKey];
                if (!string.IsNullOrEmpty(postToken) && postToken.Equals(storedToken))
                {
                    // CSRF token verification passed
                    return;
                }

                throw new CsrfVerificationException();
            }
            
            // Set new csrf token
            request.Session?.Set(CsrfKey, CreateToken(request));
        }

        public void Handle(IRequest request, Response response)
        {
            var httpResponse = response as HttpResponse;
            if (_cookieCheck && httpResponse != null)
            {
                var token = GetToken(request);
                if (!string.IsNullOrEmpty(token))
                {
                    httpResponse.Headers.Add("Set-Cookie",
                        $"{CsrfKey}={token}; Expires=0; Path=/; HttpOnly");
                }
            }
        }

        public static string GetToken(IRequest request)
        {
            return request.Session?.Get<string>(CsrfKey);
        }

        private string CreateToken(IRequest request)
        {
            var str = DateTime.Now.ToFileTime() + request.Session.Key + _salt;
            var hash = new StringBuilder();
            var crypto = _crypt.ComputeHash(Encoding.ASCII.GetBytes(str));
            foreach (var b in crypto)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
