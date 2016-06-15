using System;

namespace Shine.Errors
{
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