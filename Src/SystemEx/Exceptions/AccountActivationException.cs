using System;

namespace Amido.Common.Exceptions
{
    public class AccountActivationException : Exception
    {
        public AccountActivationException()
        {}

        public AccountActivationException(String message)
            : base(message)
        {}

        public AccountActivationException(String message, Exception innerException)
            : base(message, innerException)
        {}
    }
}
