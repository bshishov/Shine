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
        private readonly bool _sendCookie;

        public CsrfMiddleware(string salt, bool sendCsrfInCookie = true)
        {
            _sendCookie = sendCsrfInCookie;
            _crypt = new SHA256Managed();
            _salt = salt;
        }

        public void Handle(IRequest request)
        {
            var isTrustedMethod = _trustedMethods.Contains(request.Method);

            // if method requires CSRF token verification
            if (!isTrustedMethod)
            {
                var givenToken = request.Session?.Get<string>(CsrfKey);
                var passedToken = request.PostArgs[CsrfKey];
                if (givenToken != null && passedToken != null && passedToken.Equals(givenToken))
                {
                    // CSRF token verification passed
                    return;
                }

                throw new CsrfVerificationException();
            }
            
            // Set new csrftokne
            var token = CreateToken(request); 
            request.Session?.Set(CsrfKey, token);
        }

        public void Handle(IRequest request, Response response)
        {
            var httpResponse = response as HttpResponse;
            if (_sendCookie && httpResponse != null)
            {
                var token = GetToken(request);
                if (!string.IsNullOrEmpty(token))
                {
                    httpResponse.Headers.Add("Set-Cookie",
                        $"{CsrfKey}={GetToken(request)}; Expires=2000000000; Path=/; HttpOnly");
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
