using System;

namespace Amido.SystemEx
{
    public class GuidFactory : IGuidFactory
    {
        public virtual Guid GetNewIdentity()
        {
            return Guid.NewGuid();
        }
    }
}
