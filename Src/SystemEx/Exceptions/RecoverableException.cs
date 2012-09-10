using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amido.Common.Exceptions
{
    using System.Runtime.Serialization;

    public class RecoverableException : Exception
    {
        public RecoverableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RecoverableException(string message) : base(message)
        {
        }

        public RecoverableException()
            : base()
        {
        }

        protected RecoverableException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
