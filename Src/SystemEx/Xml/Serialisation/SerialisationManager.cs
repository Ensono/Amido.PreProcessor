using System.IO;
using System.Xml.Serialization;
using Amido.SystemEx.IO;
using Amido.SystemEx.Contracts;

namespace Amido.SystemEx.Xml.Serialisation
{
    public class SerialisationManager : ISerialisationManager
    {
        private IFileSystem fileSystem;

        public SerialisationManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public T DeserializeXmlFile<T>(string filePath)
        {
            Args.NotNull(fileSystem, "fileSystem");
            Args.NotNullOrEmpty(filePath, "file");
            if (!fileSystem.FileExists(filePath))
            {
                throw new FileNotFoundException(string.Format("File not found at {0}.", filePath), filePath);
            }

            T config;
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            using (var reader = fileSystem.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                config = (T) serializer.Deserialize(reader);
                reader.Close();
            }

            return config;
        }

        public void SerializeToXmlFile(string filePath, object o)
        {
            Args.NotNull(fileSystem, "fileSystem");
            Args.NotNullOrEmpty(filePath, "file");
            Args.NotNull(o, "o");

            XmlSerializer serializer = new XmlSerializer(o.GetType());
            using (var writer = fileSystem.OpenFile(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.Serialize(writer, o);
            }
        }
    }
}
