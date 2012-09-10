using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Amido.PreProcessor.Cmd
{
    public class Tokeniser : ITokeniser
    {
        public string TryReplace(string template, IDictionary<string, string> dictionary, out IList<string> tokensNotFound)
        {
            if (string.IsNullOrEmpty(template)) throw new ArgumentNullException("template");
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            const string regMatchExMask = @"(?<=\[\%)[^[]+?(?=\%\])";
            const string regExReplaceMask = @"\[\%{0}\%\]";

            var matches = Regex.Matches(template, regMatchExMask, RegexOptions.IgnoreCase);

            tokensNotFound = new List<string>();

            while (matches.Count > 0 & tokensNotFound.Count == 0)
            {
                var alreadyMatched = new List<string>();

                foreach (var match in matches)
                {
                    var token = match.ToString();

                    if (!alreadyMatched.Contains(token))
                    {
                        if (dictionary.ContainsKey(token))
                        {
                            template = Regex.Replace(template, string.Format(regExReplaceMask, token),
                                                     dictionary[token]);

                            alreadyMatched.Add(token);
                        }
                        else
                        {
                            tokensNotFound.Add(token);
                        }
                    }
                }

                matches = Regex.Matches(template, regMatchExMask, RegexOptions.IgnoreCase);
            }

            return template;
        }
    }
}
