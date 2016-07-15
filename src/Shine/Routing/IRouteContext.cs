using Shine.Responses;

namespace Shine.Routing
{
    public interface IRouteContext
    {
        IResponse Proceed(IRequest request);
    }
}