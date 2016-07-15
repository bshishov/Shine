using System;
using System.IO;
using Shine.Responses;
using Shine.Routing;
using MimeTypes;
using Shine.Http;

namespace Shine.Utilities
{
    public class StaticServeRouter : Router
    {
        private readonly string _basePath;

        public StaticServeRouter(string path, string basePath)
            : base(path)
        { 
            _basePath = basePath;
            Routables.Add(new RouteWithArg(path + "/(.+)$", Handler));
        }

        private IResponse Handler(IRequest request, string[] args)
        {
            var filePath = args[0];
            var fullPath = Path.Combine(_basePath, filePath);
            var mime = MimeTypeMap.GetMimeType(Path.GetExtension(fullPath));
            var response = new StreamedHttpResponse(File.OpenRead(fullPath), contenttype: mime);
            response.Headers.Add(StandartHttpHeaders.Expires, DateTime.Now.AddDays(1).ToString("R"));
            response.Headers.Add(StandartHttpHeaders.CacheControl, "public");
            return response;
        }
    }
}
