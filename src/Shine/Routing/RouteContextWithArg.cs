using Shine.Responses;

namespace Shine.Routing
{
    internal class RouteContextWithArg : IRouteContext
    {
        private readonly string[] _args;
        private readonly RouteWithArg _route;

        public RouteContextWithArg(RouteWithArg route, string[] args)
        {
            _args = args;
            _route = route;
        }

        public Response Proceed(IRequest request)
        {
            return _route.Proceed(request, _args);
        }
    }
}