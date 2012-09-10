using System;
using System.Net;
using System.Runtime.Serialization;

namespace Amido.SystemEx.ServiceModel
{
    /// <summary>
    /// Exception thrown by a service, optionally reporting an 
    /// <see cref="HttpStatusCode"/> to the client.
    /// </summary>
    public class ServiceException : Exception
    {
        private readonly HttpStatusCode statusCode;

        public ServiceException()
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ServiceException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            this.statusCode = statusCode;
        }

        protected ServiceException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
        }
    }
}
