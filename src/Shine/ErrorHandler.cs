using System;
using Shine.Responses;

namespace Shine
{
    public delegate IResponse ErrorHandler(IRequest request, int code, Exception ex);
}