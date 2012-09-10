using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Amido.Common.Exceptions
{
    public class MissingMappingAttributeException : Exception
    {
        public MissingMappingAttributeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MissingMappingAttributeException(string message) : base(message)
        {
        }

        public MissingMappingAttributeException()
            : base()
        {
        }

        protected MissingMappingAttributeException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
