using System.IO;
using System.Xml.Serialization;

namespace EducationalProgram_PaskalProcAndFunc
{
    public class XmlHelper
    {
        public T ReadFromXML<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamReader(path))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        public void WriteToXML<T>(T element, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamWriter(path))
            {
                serializer.Serialize(stream, element);
            }
        }
    }
}