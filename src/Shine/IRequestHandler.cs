using Shine.Responses;

namespace Shine
{
    public interface IRequestHandler
    {
        IResponse Handle(IRequest request);
    }
}