using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Amido.System.ServiceModel
{
    public sealed class DisposableChannel<T> : IDisposable
    {
        private readonly T channel;

        public DisposableChannel()
        {
            var channelFactory = new ChannelFactory<T>("*");
            channel = channelFactory.CreateChannel();
        }

        public DisposableChannel(ChannelFactory<T> channelFactory)
        {
            Contract.Requires<NullReferenceException>(channelFactory != null);
            
            channel = channelFactory.CreateChannel();
        }

        public DisposableChannel(string endpointConfigurationName)
        {
            var channelFactory = new ChannelFactory<T>(endpointConfigurationName);
            channel = channelFactory.CreateChannel();
        }

        public T Channel
        {
            get { return channel; }
        }

        public void Dispose()
        {
            var clientChannel = channel as IClientChannel;
            if (clientChannel != null)
            {
                if (clientChannel.State == CommunicationState.Opened)
                {
                    clientChannel.Dispose();
                }
                else
                {
                    clientChannel.Abort();
                }
            }
        }
    }
}
