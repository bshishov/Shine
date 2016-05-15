using System;
using System.IO;

namespace Shine.Responses
{
    public abstract class Response : IDisposable
    {
        public abstract void WriteBody(Stream stream);
        public abstract byte[] GetBody();
        public abstract void Dispose();
    }
}