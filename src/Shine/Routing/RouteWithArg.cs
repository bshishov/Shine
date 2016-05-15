using System.Text.RegularExpressions;
using Shine.Responses;

namespace Shine.Routing
{
    public class RouteWithArg : IRoute
    {
        private readonly RequestHandlerWithArg _handler;

        public RouteWithArg(string pattern, RequestHandlerWithArg handler)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _handler = handler;
        }

        public Regex Regex { get; }

        public Response Proceed(IRequest request, string[] args)
        {
            return _handler?.Invoke(request, args);
        }
    }
}