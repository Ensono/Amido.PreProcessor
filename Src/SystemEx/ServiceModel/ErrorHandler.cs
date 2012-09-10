using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using Amido.System.ServiceModel.Web;

namespace Amido.System.ServiceModel
{
    public class ErrorHandler : IErrorHandler
    {
        private const string FaultNs = "http://code.google.com/p/netfx/";
        private IWebOperationContext operationContext;

        public ErrorHandler()
        {
        }

        public IWebOperationContext Context
        {
            get
            {
                if (this.operationContext == null && WebOperationContext.Current != null)
                {
                    this.operationContext = new WebOperationContextWrapper(WebOperationContext.Current);
                }

                return this.operationContext;
            }

            set
            {
                this.operationContext = value;
            }
        }

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Contract.Requires<NullReferenceException>(error != null);

            FaultCode code = FaultCode.CreateSenderFaultCode(error.GetType().Name, FaultNs);
            fault = Message.CreateMessage(version, code, error.Message, null);

        if (this.Context != null)
        {
            var se = error as ServiceException;
            if (se != null)
            {
                Context.OutgoingResponse.StatusCode = ((ServiceException)error).StatusCode;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < error.Message.Length; i++)
            {
                var ch = (char)('\x00ff' & error.Message[i]);
                if (((ch <= '\x001f') && (ch != '\t')) || (ch == '\x007f'))
                {
                    // Specified value has invalid Control characters.
                    // See HttpListenerResponse.StatusDescription implementation.
                }
                else
                {
                    sb.Append(ch);
                }
            }

            Context.OutgoingResponse.StatusDescription = sb.ToString();
            Context.OutgoingResponse.SuppressEntityBody = false;
            }
        }
    }
}

