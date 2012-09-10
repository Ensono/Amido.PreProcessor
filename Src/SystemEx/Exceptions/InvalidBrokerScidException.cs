using System;
using System.Runtime.Serialization;

namespace Amido.Common.Exceptions
{
    public class InvalidBrokerScidException : Exception
    {
        public InvalidBrokerScidException (string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidBrokerScidException (string message) : base(message)
        {
        }

        public InvalidBrokerScidException ()
            : base()
        {
        }

        protected InvalidBrokerScidException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}