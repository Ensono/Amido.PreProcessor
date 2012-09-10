using System.ServiceModel.Web;

namespace Amido.SystemEx.ServiceModel.Web
{
    public class WebOperationContextWrapper : IWebOperationContext
    {
        private readonly WebOperationContext context;

        public WebOperationContextWrapper(WebOperationContext context)
        {
            this.context = context;
        }

        public IIncomingWebRequestContext IncomingRequest
        {
            get { return new IncomingWebRequestContextWrapper(context.IncomingRequest); }
        }

        public IOutgoingWebResponseContext OutgoingResponse
        {
            get { return new OutgoingWebResponseContextWrapper(context.OutgoingResponse); }
        }
    }
}