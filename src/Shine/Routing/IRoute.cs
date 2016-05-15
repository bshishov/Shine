using System.Text.RegularExpressions;

namespace Shine.Routing
{
    public interface IRoute
    {
        Regex Regex { get; }
    }
}