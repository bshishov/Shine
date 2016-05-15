using System;

namespace Shine.Errors
{
    public class HttpErrorException : Exception
    {
        public HttpErrorException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpErrorException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpErrorException(int statusCode, string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; private set; }
    }

    public class Http404Exception : HttpErrorException
    {
        public Http404Exception(string message = "") : base(404, message)
        {
        }
    }

    public class Http500Exception : HttpErrorException
    {
        public Http500Exception(string message = "")
            : base(500, message)
        {
        }

        public Http500Exception(Exception inner, string message = "")
            : base(500, message, inner)
        {
        }
    }
}