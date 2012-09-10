using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Amido.System.ServiceModel
{
    public class ErrorHanderBehavior : WebHttpBehavior
    {
        private readonly IErrorHandler errorHandler;

        public ErrorHanderBehavior(IErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(errorHandler);
        }
    }
}