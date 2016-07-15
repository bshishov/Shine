using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Shine.Http.Cookie;
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
            var isTrustedMethod = _trustedMethods.Contains(request.Method, StringComparer.OrdinalIgnoreCase);

            // if method requires CSRF token verification
            if (!isTrustedMethod)
            {
                var storedToken = request.Session?.Get<string>(CsrfKey);
                if(storedToken == null)
                    throw new CsrfVerificationException("CSRF token is not set");

                if (_cookieCheck)
                {
                    var cookieToken = request.Cookies?[CsrfKey];
                    if (!string.IsNullOrEmpty(cookieToken))
                    {
                        if (cookieToken.Equals(storedToken))
                            // CSRF token verification passed
                            return;

                        // Tokens mismatch
                        throw new CsrfVerificationException("Cookie CSRF token os not equal to stored CSRF token");
                    }
                }

                var postToken = request.PostArgs[CsrfKey];
                if (!string.IsNullOrEmpty(postToken))
                {
                    if(postToken.Equals(storedToken))
                        // CSRF token verification passed
                        return;

                    // Tokens mismatch
                    throw new CsrfVerificationException("POST CSRF token is not equal to stored CSRF token");
                }

                throw new CsrfVerificationException("CSRF token is not passed");
            }
            
            // Set new csrf token
            request.Session?.Set(CsrfKey, CreateToken(request));
        }

        public void Handle(IRequest request, IResponse response)
        {
            var httpResponse = response as HttpResponse;
            if (_cookieCheck && httpResponse != null)
            {
                var token = GetToken(request);
                if (!string.IsNullOrEmpty(token))
                {
                    httpResponse.AddCookie(new Cookie(CsrfKey, token, httpOnly: true));
                }
            }
        }

        public static string GetToken(IRequest request)
        {
            return request.Session?.Get<string>(CsrfKey);
        }

        private string CreateToken(IRequest request)
        {   
            var str = string.Concat(DateTime.Now.ToFileTime(), request.Session.Key, _salt);
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
