﻿using Shine.Errors;

namespace Shine.Middleware.CSRF
{
    public class CsrfVerificationException : Http403Exception
    {
        public CsrfVerificationException() : base("CSRF verification failure")
        {
        }

        public CsrfVerificationException(string message) : base("CSRF verification failure: " + message)
        {
        }
    }
}
