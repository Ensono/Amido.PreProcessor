using System;
using System.Runtime.Serialization;

namespace Amido.Common.Exceptions
{
    public class InsurerLookupException : Exception
    {
        public InsurerLookupException (string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InsurerLookupException (string message) : base(message)
        {
        }

        public InsurerLookupException ()
            : base()
        {
        }

        protected InsurerLookupException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}