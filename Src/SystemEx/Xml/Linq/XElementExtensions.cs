using System;
using System.Linq;
using System.Xml.Linq;

namespace Amido.SystemEx.Xml.Linq
{
    public sealed class XElementExtensions
    {
        private XElementExtensions() {}

        public static decimal SafeGetDecimalFromElement(XElement element, string elementName)
        {
            XElement x = GetElement(element, elementName);

            decimal result;

            if (!decimal.TryParse(x.Value, out result))
            {
                throw new InvalidOperationException(string.Format("Decimal not found in element {0}.", elementName));
            }

            return result;
        }

        public static bool ElementExists(XContainer element, string elementName)
        {
            return element.Descendants(elementName).Any();
        }

        public static int SafeGetIntFromElement(XElement element, string elementName)
        {
            XElement x = GetElement(element, elementName);

            int result;

            if (!int.TryParse(x.Value, out result))
            {
                throw new InvalidOperationException(string.Format("Decimal not found in element {0}.", elementName));
            }

            return result;
        }

        public static string SafeGetStringFromElement(XElement element, string elementName)
        {
            XElement x = GetElement(element, elementName);

            var result = String.Empty;

            if (x.Value != null)
            {
                result = x.Value;
            }

            return result;
        }

        public static decimal SafeGetDecimalFromAttribute(XElement element, string elementName, string attributeName)
        {
            XAttribute a = GetAttribute(GetElement(element, elementName), attributeName);

            decimal result;

            if (!decimal.TryParse(a.Value, out result))
            {
                throw new InvalidOperationException(string.Format("Decimal not found in attribute {0} for element {1}.", attributeName, elementName));
            }

            return result;
        }

        public static decimal SafeGetDecimalFromAttribute(XElement element, string attributeName)
        {
            XAttribute a = GetAttribute(element, attributeName);

            decimal result;

            if (!decimal.TryParse(a.Value, out result))
            {
                throw new InvalidOperationException(string.Format("Decimal not found in attribute {0} on element {1}.", attributeName, element.Name));
            }

            return result;
        }

        public static int SafeGetIntFromAttribute(XElement element, string elementName, string attributeName)
        {
            XAttribute a = GetAttribute(GetElement(element, elementName), attributeName);

            int result;

            if (!int.TryParse(a.Value, out result))
            {
                throw new InvalidOperationException(string.Format("Integer not found in attribute {0} for element {1}.", attributeName, elementName));
            }

            return result;
        }

        public static int SafeGetIntFromAttribute(XElement element, string attributeName)
        {
            XAttribute a = GetAttribute(element, attributeName);

            int result;

            if (!int.TryParse(a.Value, out result))
            {
                throw new InvalidOperationException(string.Format("Integer not found in attribute {0} on element {1}", attributeName, element.Name));
            }

            return result;
        }

        public static string SafeGetStringFromAttribute(XElement element, string elementName, string attributeName)
        {
            XAttribute a = GetAttribute(GetElement(element, elementName), attributeName);

            var result = String.Empty;

            if (a.Value != null)
            {
                result = a.Value;
            }

            return result;
        }

        public static string SafeGetStringFromAttribute(XElement element, string attributeName)
        {
            XAttribute a = GetAttribute(element, attributeName);

            var result = string.Empty;

            if(a.Value != null)
            {
                result = a.Value;
            }

            return result;
        }

        private static XElement GetElement(XElement element, string elementName)
        {
            var x = element.Descendants(elementName).FirstOrDefault();
            if (x == null)
            {
                throw new InvalidOperationException(string.Format("Element {0} not found.", elementName));
            }

            return x;
        }

        private static XAttribute GetAttribute(XElement element, string attributeName)
        {
            var x = element.Attribute(attributeName);
            if (x == null)
            {
                throw new InvalidOperationException(string.Format("Attribute {0} not found.", attributeName));
            }

            return x;
        }
    }
}
