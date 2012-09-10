using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amido.Common.Exceptions
{
    using System.Runtime.Serialization;

    public class XsdSchemaNotValidException : Exception
    {
        public XsdSchemaNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public XsdSchemaNotValidException(string message) : base(message)
        {
        }

        public XsdSchemaNotValidException()
            : base()
        {
        }

        protected XsdSchemaNotValidException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
