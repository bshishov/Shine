using System;
using System.Diagnostics;
using FastCGI;
using Shine.Responses;
using Shine.Server;

namespace Shine.FastCGI
{
    public class FastCGIServer : IServer
    {
        private readonly FCGIApplication _fcgiApplication;
        private IAsyncRequestHandler _handler;
        private readonly int _port;

        public FastCGIServer(int port)
        {
            _port = port;
            _fcgiApplication = new FCGIApplication();
            _fcgiApplication.OnRequestReceived += FcgiApplicationOnOnRequestReceived;
        }

        private void FcgiApplicationOnOnRequestReceived(object sender, Request request)
        {
            var response = _handler.HandleAsync(new FastCGIRequestWrapper(request)).Result;

            if (response == null)
                return;

            using (var httpResponse = response as HttpResponse)
            {
                try
                {
                    if (httpResponse != null)
                        request.WriteResponse(httpResponse.GetHeader());

                    request.WriteResponse(response.GetBody());
                    request.Close();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }

        public void Run(IAsyncRequestHandler handler)
        {
            Debug.Assert(handler != null);
            _handler = handler;
            _fcgiApplication.Run(_port);
        }

        public void Stop()
        {
            _fcgiApplication.StopListening();
        }
    }
}
