using System.Text.RegularExpressions;
using Shine.Responses;

namespace Shine.Routing
{
    public class Route : IRoute
    {
        private readonly RequestHandler _handler;

        public Route(string pattern, RequestHandler handler)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _handler = handler;
        }

        public Regex Regex { get; }

        public Response Proceed(IRequest request)
        {
            if (_handler != null)
                return _handler(request);

            return null;
        }
    }
}