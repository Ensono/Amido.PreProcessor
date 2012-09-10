using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amido.Common.Exceptions
{
    using System.Runtime.Serialization;

    public class XsdSchemaNotFoundException : Exception
    {
        public XsdSchemaNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public XsdSchemaNotFoundException(string message) : base(message)
        {
        }

        public XsdSchemaNotFoundException()
            : base()
        {
        }

        protected XsdSchemaNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
