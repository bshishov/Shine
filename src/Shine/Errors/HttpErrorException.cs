using System;

namespace Shine.Errors
{
    public class HttpErrorException : ShineException
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
}