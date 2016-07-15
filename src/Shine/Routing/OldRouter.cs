﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
namespace Shine.Routing
{
    [Obsolete]
    public class OldRouter
    {
        private readonly Dictionary<string, OldRouter> _routers = new Dictionary<string, OldRouter>();
        private readonly List<IRoutable> _routes;

        public OldRouter(List<IRoutable> routes)
        {
            _routes = routes;
        }

        public OldRouter() : this(new List<IRoutable>())
        {
        }

        public void Bind(string pattern, RequestHandler handler)
        {
            _routes.Add(new Route(pattern, handler));
        }

        public void Bind(string pattern, RequestHandlerWithArg handler)
        {
            _routes.Add(new RouteWithArg(pattern, handler));
        }

        public IRouteContext GetRoute(string documentUri)
        {
            foreach (var route in _routes)
            {
                var match = route.Regex.Match(documentUri);
                if (match.Length > 0)
                {
                    if (match.Groups.Count > 1)
                    {
                        var vals = new string[match.Groups.Count - 1];
                        for (var i = 1; i < match.Groups.Count; i++)
                            vals[i - 1] = WebUtility.UrlDecode(match.Groups[i].Value);

                        var rarg = route as RouteWithArg;
                        if (rarg != null)
                            return new RouteContextWithArg(rarg, vals);

                        return new RouteContext((Route) route);
                    }
                    return new RouteContext((Route) route);
                }
            }

            // delegate to other OldRouter
            foreach (var kvp in _routers)
            {
                if (Regex.IsMatch(documentUri, kvp.Key))
                {
                    var newUri = Regex.Replace(documentUri, kvp.Key, "");
                    var routeCtx = kvp.Value.GetRoute(newUri);
                    if (routeCtx != null)
                        return routeCtx;
                }
            }

            return null;
        }

        public void Bind(string pattern, OldRouter router)
        {
            _routers.Add(pattern, router);
        }
    }
}