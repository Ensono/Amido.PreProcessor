using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amido.Common.Exceptions
{
    using System.Runtime.Serialization;

    public class XmlDidNotPassValidationException : Exception
    {
        public XmlDidNotPassValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public XmlDidNotPassValidationException(string message) : base(message)
        {
        }

        public XmlDidNotPassValidationException()
            : base()
        {
        }

        protected XmlDidNotPassValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
