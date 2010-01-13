using System.ServiceModel;
using System.ServiceModel.Activation;
using TwitDegrees.Presentation.Core.Models;
using TwitDegrees.Presentation.Core.Services;

namespace TwitDegrees.Web.Api
{
    [ServiceContract(Namespace = "TwitDegrees.Web.Api")]
    public interface ITwitter
    {
        
        [OperationContract]
        string GetUserGraphML(string name);
    }


    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Twitter : ITwitter
    {
        

        public string GetUserGraphML(string name)
        {
            // TODO injection
            return new GraphService().BuildGraphML(name);
        }
    }
}
