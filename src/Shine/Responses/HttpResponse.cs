using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Shine.Http;
using Shine.Http.Cookie;

namespace Shine.Responses
{
    public class HttpResponse : IResponse
    {
        public HttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

        protected byte[] Content;
        private readonly string _statusReason;

        public HttpResponse(byte[] content, int status = 200, string statusReason = "OK",
            string contenttype = "text/html", Dictionary<string, string> headers = null)
        {
            StatusCode = status;
            if(!string.IsNullOrEmpty(contenttype))
                Headers.Add(StandartHttpHeaders.ContentType, contenttype);
            if (headers != null)
            {
                foreach (var attribute in headers)
                    Headers.Add(attribute.Key, attribute.Value);
            }
            _statusReason = statusReason;
            Content = content;
        }

        public HttpResponse(string content = "", int status = 200, string statusReason = "OK",
            string contenttype = "text/html", Dictionary<string, string> headers = null)
        {
            StatusCode = status;
            if (!string.IsNullOrEmpty(contenttype))
                Headers.Add(StandartHttpHeaders.ContentType, contenttype);
            if (headers != null)
            {
                foreach (var attribute in headers)
                    Headers.Add(attribute.Key, attribute.Value);
            }
            _statusReason = statusReason;
            Content = Encoding.UTF8.GetBytes(content);
        }

        public int StatusCode { get; }

        public byte[] GetHeader()
        {
            using (var ms = new MemoryStream())
            {
                WriteString(ms, $"HTTP/1.1 {StatusCode} {_statusReason}");

                // TODO: SERIALIZE httpheader collection!
                //var headers = Headers.ToByteArray();
                //ms.Write(headers, 0, headers.Length);

                WriteString(ms, "\r\n");
                ms.Close();
                return ms.ToArray();
            }
        }

        private static void WriteString(Stream stream, string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            stream.Write(data, 0, data.Length);
        }

        public virtual void WriteBodyToStream(Stream stream)
        {
            try
            {
                stream.Write(Content, 0, Content.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public virtual byte[] GetBody()
        {
            return Content;
        }

        public virtual void Dispose()
        {
        }

        public void AddCookie(string cookieStr)
        {
            // Set-cookie header should not fold
            Headers.Add(new HttpHeaderField(StandartHttpHeaders.SetCookie, cookieStr), folding: false);
        }

        public void AddCookie(ICookie cookie)
        {
            AddCookie(cookie.ToString());
        }
    }
}