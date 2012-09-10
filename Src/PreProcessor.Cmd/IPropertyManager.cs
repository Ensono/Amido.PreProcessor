using System.Collections.Generic;

namespace Amido.PreProcessor.Cmd
{
    public interface IPropertyManager
    {   
        IDictionary<string, string> LoadProperties(string propertyFile);
        IDictionary<string, string> LoadProperties(string propertyFile, string overrideFile);
    }
}