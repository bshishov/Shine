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
                using (var response = await _handler.Execute(new HttpRequestWrapper(context.Request)) as HttpResponse)
                {
                    if (response != null)
                    {
                        context.Response.StatusCode = response.StatusCode;

                        foreach (var key in response.Headers.AllKeys)
                            context.Response.AddHeader(key, response.Headers[key]);
                        
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
