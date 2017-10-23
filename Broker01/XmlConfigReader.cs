using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BrokerAlgo
{
    public class XmlConfigReader
    {
        private readonly string fileName;

        public XmlConfigReader(string fileName)
        {
            this.fileName = fileName;
        }

        public List<string> ReadList()
        {
            var result = new List<string>();
            var text = File.ReadAllText(fileName);
            using (var reader = XmlReader.Create(new StringReader(text)))
            {
                while (reader.ReadToFollowing("i"))
                {
                    reader.Read();
                    var value = reader.Value;
                    result.Add(value);
                }
            }
            return result;
        }
    }
}
