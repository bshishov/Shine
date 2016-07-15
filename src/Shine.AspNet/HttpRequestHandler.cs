using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HttpResponse = Shine.Responses.HttpResponse;

namespace Shine.AspNet
{
    class HttpRequestHandler : HttpTaskAsyncHandler
    {
        private readonly IAsyncRequestHandler _handler;

        public HttpRequestHandler()
        {
            _handler = new App(null);
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            Debug.Assert(context != null);
            Debug.Assert(_handler != null);

            try
            {
                using (var response = await _handler.HandleAsync(new HttpRequestWrapper(context.Request)) as HttpResponse)
                {
                    if (response != null)
                    {
                        context.Response.StatusCode = response.StatusCode;

                        foreach (var header in response.Headers)
                            context.Response.AddHeader(header.Name, header.Value);
                        
                        response.WriteBodyToStream(context.Response.OutputStream);
                    }
                }
                context.Response.Close();
            }
            catch (HttpListenerException)
            {
                // Ignored.
            }
            catch (Exception ex)
            {
                // TODO: better exception handling
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}
