using System;
using System.IO;
using Shine.Responses;
using Shine.Routing;
using MimeTypes;

namespace Shine.Utilities
{
    public class StaticServeRouter : Router
    {
        private readonly string _basePath;

        public StaticServeRouter(string basePath)
        { 
            _basePath = basePath;
            this.Bind("/(.+)$", Handler);
        }

        private Response Handler(IRequest request, string[] args)
        {
            var filePath = args[0];
            var fullPath = Path.Combine(_basePath, filePath);
            var mime = MimeTypeMap.GetMimeType(Path.GetExtension(fullPath));
            var response = new StreamedHttpResponse(File.OpenRead(fullPath), contenttype: mime);
            response.Headers.Add("Expires", DateTime.Now.AddDays(1).ToString("R"));
            response.Headers.Add("Cache-Control", "public");
            return response;
        }
    }
}
