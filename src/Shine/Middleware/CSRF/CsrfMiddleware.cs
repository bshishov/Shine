using System;
using System.Linq;
using Shine.Errors;
using Shine.Responses;

namespace Shine.Middleware.CSRF
{
    public class CsrfMiddleware : IMiddleware
    {
        public const string CsrfKey = "csrf_token";
        private readonly string[] _trustedMethods = { "GET", "TRACE", "HEAD", "OPTIONS" };

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

                throw new HttpErrorException(403, "CSRF token verification fail");
            }
            
            // Set new csrftokne
            var token = CreateToken(request); 
            request.Session?.Set(CsrfKey, token);
        }

        public void Handle(IRequest request, Response response)
        {
            var httpResponse = response as HttpResponse;
            if (httpResponse != null)
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
            return DateTime.Now.ToFileTime() + request.Session.Key + "_SUCH_TOKEN_MUCH_WOW";
        }
    }
}
