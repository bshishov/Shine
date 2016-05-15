using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Shine.Errors;
using Shine.Middleware;
using Shine.Responses;
using Shine.Routing;
using Shine.Templating;
using Shine.Utilities;

namespace Shine
{
    public class App : IAsyncRequestHandler
    {
        public static ITemplateProcessor TemplateProcessor { get; private set; }
        public ErrorHandler ErrorHandler;
        
        private readonly Pipeline<IRequest, Response> _afterView = new Pipeline<IRequest, Response>();
        private readonly Pipeline<IRequest> _beforeView = new Pipeline<IRequest>();
        private readonly Router _rootRouter;

        public App(Router defaultRouter)
        {
            _rootRouter = defaultRouter;
        }

        public void SetTemplateProcessor(ITemplateProcessor templateProcessor)
        {
            App.TemplateProcessor = templateProcessor;
        }

        public void RegisterMiddleware(IMiddleware middleware)
        {
            _beforeView.Add(middleware);
            _afterView.Insert(0, middleware);
        }

        public async Task<Response> Execute(IRequest request)
        {
            try
            {
#if DEBUG
                var stopWatch = new Stopwatch();
                stopWatch.Start();
#endif
                _beforeView.Run(request);

                var routeContext = _rootRouter.GetRoute(request.Path);

                if (routeContext != null)
                {
                    var response = routeContext.Proceed(request);
                    _afterView.Run(request, response);

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

                throw new Http404Exception("No route found");
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