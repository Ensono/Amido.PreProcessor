using Amido.SystemEx.IO;

namespace Amido.SystemEx.Xml.Serialisation
{
    public interface ISerialisationManager
    {
        T DeserializeXmlFile<T>(string filePath);
        void SerializeToXmlFile(string filePath, object o);
    }
}
