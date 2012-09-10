using System;

namespace Amido.SystemEx
{
    public interface IGuidFactory
    {
        Guid GetNewIdentity();
    }
}