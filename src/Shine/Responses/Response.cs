using System;
using System.IO;

namespace Shine.Responses
{
    public interface IResponse : IDisposable
    {
        void WriteBodyToStream(Stream stream);
        byte[] GetBody();
    }
}