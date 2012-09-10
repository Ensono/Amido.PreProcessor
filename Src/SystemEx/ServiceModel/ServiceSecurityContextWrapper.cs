using System.IdentityModel.Policy;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.ServiceModel;

namespace Amido.SystemEx.ServiceModel
{
    public sealed class ServiceSecurityContextWrapper : IServiceSecurityContext
    {
        private readonly ServiceSecurityContext context;

        public ServiceSecurityContextWrapper(ServiceSecurityContext context)
        {
            this.context = context;
        }

        AuthorizationContext IServiceSecurityContext.AuthorizationContext
        {
            get
            {
                return this.context.AuthorizationContext;
            }
        }

        ReadOnlyCollection<IAuthorizationPolicy> IServiceSecurityContext.AuthorizationPolicies
        {
            get
            {
                return this.context.AuthorizationPolicies;
            }
        }

        bool IServiceSecurityContext.IsAnonymous
        {
            get
            {
                return this.context.IsAnonymous;
            }
        }

        IIdentity IServiceSecurityContext.PrimaryIdentity
        {
            get
            {
                return this.context.PrimaryIdentity;
            }
        }

        WindowsIdentity IServiceSecurityContext.WindowsIdentity
        {
            get
            {
                return this.context.WindowsIdentity;
            }
        }
    }
}
