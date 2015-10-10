using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Ahk
{
    public static class XmlStorage
    {
        public static T Load<T>(string file)
        {
            if (!File.Exists(file))
            {
                return default(T);
            }

            var readerSettings = new XmlReaderSettings { IgnoreWhitespace = true };
            using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (XmlReader reader = XmlReader.Create(fs, readerSettings))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        public static void Save(string file, object obj)
        {
            var settings = new XmlWriterSettings { Indent = true };

            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, settings))
            {
                var serializer = new XmlSerializer(obj.GetType());
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                serializer.Serialize(writer, obj, namespaces);
                File.WriteAllBytes(file, ms.ToArray());
            }
        }
    }
}