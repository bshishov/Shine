using Shine.Responses;
using Shine.Utilities;

namespace Shine.Middleware
{
    public interface IMiddleware : IHandler<IRequest>, IHandler<IRequest, Response>
    {
    }
}