using System.Text.RegularExpressions;

namespace Shine.Routing
{
    public interface IRoutable : IRequestHandler
    {
        Regex Regex { get; }
    }
}