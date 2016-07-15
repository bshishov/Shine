using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

using Shine.Errors;
using Shine.Middleware;
using Shine.Responses;
using Shine.Utilities;

namespace Shine.Routing
{
    public class Router : IRoutable
    {
        private readonly List<IRoutable> _routes;
        private readonly Pipeline<IRequest, IResponse> _afterHandler = new Pipeline<IRequest, IResponse>();
        private readonly Pipeline<IRequest> _beforeHandler = new Pipeline<IRequest>();

        public IList<IRoutable> Routables => _routes;

        public Regex Regex { get; }

        public IResponse Handle(IRequest request)
        {
            _beforeHandler.Run(request);

            string[] args;
            IResponse response;

            var routable = GetRoutable(request.Path, out args);

            if(routable == null)
                throw new Http404Exception("No routes found");

            if (args != null && routable is RouteWithArg)
                response = ((RouteWithArg) routable).Handle(request, args);
            else
                response = ((IRequestHandler) routable).Handle(request);

            _afterHandler.Run(request, response);
            return response;
        }

        private IRoutable GetRoutable(string path, out string[] args)
        {
            args = null;
            
            foreach (var route in _routes)
            {
                var match = route.Regex.Match(path);
                
                if (match.Success)
                {
                    if (match.Groups.Count > 1)
                    {
                        var vals = new string[match.Groups.Count - 1];
                        for (var i = 1; i < match.Groups.Count; i++)
                            vals[i - 1] = WebUtility.UrlDecode(match.Groups[i].Value);

                        args = vals;
                    }

                    return route;
                }
            }

            return null;
        }

        public Router(string path, params IRoutable[] routables)
        {
            _routes = new List<IRoutable>(routables);
            Regex = new Regex(path);
        }

        public Router(params IRoutable[] routables)
            : this(String.Empty, routables)
        {
        }

        public void RegisterMiddleware(IMiddleware middleware)
        {
            _beforeHandler.Add(middleware);
            _afterHandler.Insert(0, middleware);
        }
    }
}
