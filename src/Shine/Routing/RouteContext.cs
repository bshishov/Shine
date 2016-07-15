using Shine.Responses;

namespace Shine.Routing
{
    internal class RouteContext : IRouteContext
    {
        private readonly Route _route;

        public RouteContext(Route route)
        {
            _route = route;
        }

        public IResponse Proceed(IRequest request)
        {
            return _route.Handle(request);
        }
    }
}