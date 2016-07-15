using System.Collections.Generic;
using Shine.Http;

namespace Shine.Responses
{
    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string url)
            : base(status: 302, statusReason: "Found", contenttype: null, headers: new Dictionary<string, string>
            {
                {StandartHttpHeaders.Location, url}
            })
        {
        }
    }
}