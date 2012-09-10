using System;
using System.Net;
using System.ServiceModel.Web;

namespace Amido.SystemEx.ServiceModel.Web
{
    public interface IIncomingWebRequestContext
    {
        string Accept { get; }

        long ContentLength { get; }

        string ContentType { get; }

        WebHeaderCollection Headers { get; }

        string Method { get; }

        UriTemplateMatch UriTemplateMatch { get; set; }

        string UserAgent { get; }
    }

    public interface IOutgoingWebResponseContext
    {
        long ContentLength { get; set; }

        string ContentType { get; set; }

        string ETag { get; set; }

        WebHeaderCollection Headers { get; }

        DateTime LastModified { get; set; }

        string Location { get; set; }

        HttpStatusCode StatusCode { get; set; }

        string StatusDescription { get; set; }

        bool SuppressEntityBody { get; set; }

        void SetStatusAsCreated(Uri locationUri);

        void SetStatusAsNotFound();

        void SetStatusAsNotFound(string description);
    }

    public interface IWebOperationContext
    {
        IIncomingWebRequestContext IncomingRequest { get; }

        IOutgoingWebResponseContext OutgoingResponse { get; }
    }

    public class IncomingWebRequestContextWrapper : IIncomingWebRequestContext
    {
        private readonly IncomingWebRequestContext context;

        public IncomingWebRequestContextWrapper(IncomingWebRequestContext context)
        {
            this.context = context;
        }

        public string Accept
        {
            get { return context.Accept; }
        }

        public long ContentLength
        {
            get { return context.ContentLength; }
        }

        public string ContentType
        {
            get { return context.ContentType; }
        }

        public WebHeaderCollection Headers
        {
            get { return context.Headers; }
        }

        public string Method
        {
            get { return context.Method; }
        }

        public UriTemplateMatch UriTemplateMatch
        {
            get { return context.UriTemplateMatch; }
            set { context.UriTemplateMatch = value; }
        }

        public string UserAgent
        {
            get { return context.UserAgent; }
        }
    }
}