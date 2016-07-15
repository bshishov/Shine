using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Shine.Errors;
using Shine.Responses;
using Shine.Routing;
using Shine.Templating;

namespace Shine
{
    public class App : IAsyncRequestHandler
    {
        public static ITemplateProcessor TemplateProcessor { get; private set; }
        public ErrorHandler ErrorHandler;
        private readonly IRoutable _root;

        public App(IRoutable rootRoutable)
        {
            _root = rootRoutable;
        }

        public void SetTemplateProcessor(ITemplateProcessor templateProcessor)
        {
            App.TemplateProcessor = templateProcessor;
        }

        public async Task<IResponse> HandleAsync(IRequest request)
        {
            try
            {
#if DEBUG
                var stopWatch = new Stopwatch();
                stopWatch.Start();
#endif
                var response = ((IRequestHandler) _root).Handle(request);
                var httpResponse = response as HttpResponse;
                if (httpResponse != null)
                {
#if DEBUG
                    stopWatch.Stop();
                    Console.WriteLine("{0} {1} {2} {3}ms", request.Method, httpResponse.StatusCode, request.Path, stopWatch.ElapsedMilliseconds);
#else
                    Console.WriteLine("{0} {1} {2}", request.Method, httpResponse.StatusCode, request.Path);
#endif
                }

                return response;
            }
            catch (HttpErrorException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} {1} {2} {3}", request.Method, e.StatusCode, request.Path, e.Message);
                Console.ResetColor();

                return ErrorHandler?.Invoke(request, e.StatusCode, e);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} {1} {2} {3}", request.Method, 500, request.Path, e.Message);
                Console.ResetColor();

                return ErrorHandler?.Invoke(request, 500, e);
            }
        }
    }
}