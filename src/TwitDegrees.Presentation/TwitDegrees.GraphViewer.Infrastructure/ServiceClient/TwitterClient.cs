using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.Unity;
using TwitDegrees.GraphViewer.Infrastructure.Model;
using TwitDegrees.GraphViewer.Infrastructure.Services;

namespace TwitDegrees.GraphViewer.Infrastructure.ServiceClient
{
    [ServiceContract(Namespace = "TwitDegrees.Web.Api")]
    public interface ITwitter
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetUser(string name, AsyncCallback callback, object asyncState);
        TwitterUser EndGetUser(IAsyncResult result);
    }



    public interface ITwitterClient
    {
        void GetUser(string name, Action<TwitterUser> callback);
    }



    public class TwitterClient : ClientBase<ITwitter>, ITwitterClient
    {
        public TwitterClient() { }

		public TwitterClient(string endpointConfigurationName)
			: base(endpointConfigurationName) { }

		public TwitterClient(string endpointConfigurationName, string remoteAddress)
			: base(endpointConfigurationName, remoteAddress) { }

		public TwitterClient(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress) { }

        [InjectionConstructor]
        public TwitterClient(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress) { }


        protected void RaiseException(Exception ex)
        {
            // TODO: Do something smart here.
            Console.WriteLine(ex);
        }

        private static DispatchHost DispatchHost { get { return DispatchHostLocator.DispatchHost; } }


        public void GetUser(string name, Action<TwitterUser> callback)
        {
            Channel.BeginGetUser(name, result =>
            {
                try
                {
                    var returnValue = Channel.EndGetUser(result);
                    if (callback != null)
                        DispatchHost.BeginInvoke(() => callback(returnValue));
                }
                catch (Exception ex)
                {
                    RaiseException(ex);
                    throw;
                }
            }, null);
        }
    }
}
