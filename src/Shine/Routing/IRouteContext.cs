using Shine.Responses;

namespace Shine.Routing
{
    public interface IRouteContext
    {
        Response Proceed(IRequest request);
    }
}