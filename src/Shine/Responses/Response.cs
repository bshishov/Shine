using System;
using System.IO;

namespace Shine.Responses
{
    public abstract class Response : IDisposable
    {
        public abstract void WriteBodyToStream(Stream stream);
        public abstract byte[] GetBody();
        public abstract void Dispose();
    }
}