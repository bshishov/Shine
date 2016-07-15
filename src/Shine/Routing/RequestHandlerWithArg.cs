using Shine.Responses;

namespace Shine.Routing
{
    public delegate IResponse RequestHandlerWithArg(IRequest request, params string[] args);
}