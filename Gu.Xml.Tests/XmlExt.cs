namespace Gu.Xml.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    public static class XmlExt
    {
        public static T Roundtrip<T>(this T item, bool printXmlToConsole = true)
        {
            var xml = item.ToXml(printXmlToConsole);
            var roundtrip = xml.To<T>();
            return roundtrip;
        }

        public static string ToXml<T>(this T item, bool printXmlToConsole = true)
        {
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, item);
                var xml = stringBuilder.ToString();
                if (printXmlToConsole)
                {
                    Console.Write(xml);
                    Console.WriteLine();
                    Console.WriteLine();
                }
                return xml;
            }
        }

        public static T To<T>(this string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                var deserialize = (T)serializer.Deserialize(reader);
                return deserialize;
            }
        }

        public static void DumpToConsole(this Exception e, int indent = 0)
        {
            Console.WriteLine(e.GetType().Name);
            Console.Write(e.Message);
            Console.WriteLine();
            Console.WriteLine();
            if (e.InnerException != null)
            {
                e.InnerException.DumpToConsole();
            }
        }
    }
}
