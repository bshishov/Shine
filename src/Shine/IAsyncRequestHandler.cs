using System.Threading.Tasks;
using Shine.Responses;

namespace Shine
{
    public interface IAsyncRequestHandler
    {
        /// <summary>
        /// Main execution method of the handler which returns an HTTP response intent.
        /// </summary>
        /// <returns></returns>
        Task<IResponse> HandleAsync(IRequest request);
    }
}
