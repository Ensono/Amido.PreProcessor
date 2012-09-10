using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Amido.Common.Extensions
{
    public static class StringExtensions
    {
        public static string AddSpacesBeforeCapitalLetters(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if (i > 1 && !char.IsUpper(text[i+1]))
                    {
                        newText.Append(' ');
                    }
                }

                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        public static byte[] ToByteArray(this string text)
        {
            return System.Text.Encoding.Default.GetBytes(text);
        }

        public static string[] SplitEx(this string input, char separator, char escapeCharacter)
        {
            IList<string> values = new List<string>();

            int startOfSegment = 0;
            int index = 0;
            while (index < input.Length)
            {
                index = input.IndexOf(separator, index);
                if (index > 0 && input[index - 1] == escapeCharacter)
                {
                    index++;
                    continue;
                }
                if (index == -1)
                {
                    break;
                }
                values.Add(input.Substring(startOfSegment, index - startOfSegment));
                index++;
                startOfSegment = index;
            }

            values.Add(input.Substring(startOfSegment));
            return values.ToArray();
        } 
    }
}
