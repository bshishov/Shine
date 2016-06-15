using System;
using System.Runtime.Serialization;

namespace Shine.Errors
{
    public class ShineException : Exception
    {
        public ShineException()
        {
        }

        public ShineException(string message) : base(message)
        {
        }

        public ShineException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ShineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
