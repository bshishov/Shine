using System;
using System.Text.RegularExpressions;
using Shine.Responses;

namespace Shine.Routing
{
    public class RouteWithArg : IRoutable
    {
        private readonly RequestHandlerWithArg _handler;

        public RouteWithArg(string pattern, RequestHandlerWithArg handler)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _handler = handler;
        }

        public Regex Regex { get; }

        public IResponse Handle(IRequest request, string[] args)
        {
            return _handler?.Invoke(request, args);
        }

        public IResponse Handle(IRequest request)
        {
            throw new ArgumentException("Arguments required");
        }
    }
}