using System;
using System.Collections.Generic;
using System.Globalization;
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

        public List<T> ReadList<T>()
        {
            var result = new List<T>();
            var doc = new XmlDocument();
            doc.Load(fileName);

            var xmlNodeList = doc.SelectNodes("//i");
            foreach (XmlNode node in xmlNodeList)
            {
                var value = (T)Convert.ChangeType(node.InnerText, typeof(T));
                result.Add(value);
            }
            return result;
        }

        public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(string attributeNameForKey)
        {
            var result = new Dictionary<TKey, TValue>();

            var doc = new XmlDocument();
            doc.Load(fileName);

            var xmlNodeList = doc.SelectNodes("//i");
            foreach (XmlNode node in xmlNodeList)
            {
                var key = (TKey)Convert.ChangeType(node.Attributes[attributeNameForKey].InnerText, typeof(TKey));
                if (key == null)
                    throw new InvalidOperationException($"Invalid config '{fileName}': key can't be null");
                var value = (TValue)Convert.ChangeType(node.InnerText, typeof(TValue), new NumberFormatInfo {NumberDecimalSeparator = "."});
                result.Add(key, value);
            }

            return result;
        }
    }
}
