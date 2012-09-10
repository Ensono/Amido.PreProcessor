using System.Collections.Generic;

namespace Amido.PreProcessor.Cmd
{
    public interface ITokenisationRunner
    {
        void ProcessSingleTemplate(IDictionary<string, string> properties, string sourceFile, string destinationFile);
    }
}