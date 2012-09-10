using System;

namespace Amido.SystemEx.Contracts
{
    public static class Args
    {
        public static void NotNull<T>([ValidatedNotNull] T param, string argName)
        {
            if(string.IsNullOrEmpty(argName))
            {
                throw new ArgumentNullException(argName, string.Format("Argument name {0} cannot be null or empty.", argName));
            }

            if (null == param)
            {

                throw new ArgumentNullException(string.Format("Parameter {0} cannot be a null object.", argName));
            }
        }

        public static void NotNullOrEmpty(string param, string argName)
        {
            if (string.IsNullOrEmpty(argName))
            {
                throw new ArgumentNullException(argName, string.Format("Argument name {0} cannot be null or empty.", argName));
            }

            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentNullException(string.Format("Parameter {0} cannot be a null or empty string.", argName));
            }
        }

        public static void NotEmptyGuid(Guid param, string argName)
        {
            if (string.IsNullOrEmpty(argName))
            {
                throw new ArgumentNullException(argName, string.Format("Argument name {0} cannot be null or empty.", argName));
            }

            if (param.Equals(Guid.Empty))
            {
                throw new ArgumentException(string.Format("Parameter {0} cannot be an empty Guid.", argName));
            }
        }
    }
}
