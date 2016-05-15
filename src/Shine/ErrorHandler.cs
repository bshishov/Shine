using System;
using Shine.Responses;

namespace Shine
{
    public delegate Response ErrorHandler(IRequest request, int code, Exception ex);
}