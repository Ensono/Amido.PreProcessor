using System.Collections.Generic;

namespace Amido.PreProcessor.Cmd
{
    public interface ITokeniser
    {
        string TryReplace(string template, IDictionary<string, string> dictionary, out IList<string> tokensNotFound);
    }
}