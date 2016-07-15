using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shine.Responses;

namespace Shine.Routing
{
    public class Route : IRoutable
    {
        private readonly RequestHandler _handler;

        public Route(string pattern, RequestHandler handler)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _handler = handler;
        }

        public Regex Regex { get; }

        public IResponse Handle(IRequest request)
        {
            return _handler?.Invoke(request);
        }

        public Task<IResponse> HandleAsync(IRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}