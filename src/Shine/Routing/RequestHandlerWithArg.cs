using Shine.Responses;

namespace Shine.Routing
{
    public delegate Response RequestHandlerWithArg(IRequest request, params string[] args);
}