using System.Collections.ObjectModel;
using System.Security.Principal;
using System.IdentityModel.Policy;

namespace Amido.SystemEx.ServiceModel
{
    public interface IServiceSecurityContext
    {
        AuthorizationContext AuthorizationContext { get; }

        ReadOnlyCollection<IAuthorizationPolicy> AuthorizationPolicies { get; }

        bool IsAnonymous { get; }

        IIdentity PrimaryIdentity { get; }

        WindowsIdentity WindowsIdentity { get; }
    }
}
