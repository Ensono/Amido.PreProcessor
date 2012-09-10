using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amido.SystemEx.Contracts
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
