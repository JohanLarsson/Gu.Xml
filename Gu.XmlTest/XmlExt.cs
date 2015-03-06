namespace Gu.XmlTest
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public static class XmlExt
    {
        private static XmlWriterSettings Settings
        {
            get
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineHandling = NewLineHandling.Entitize,
                    OmitXmlDeclaration = true,
                    ////NamespaceHandling = NamespaceHandling.Default
                };
                return settings;
            }
        }

        public static T Roundtrip<T>(this T item, bool printXmlToConsole = true)
        {
            var list = new T[] { item, item };
            item.ToXml(printXmlToConsole);
            var listXml = list.ToXml(false);
            var roundtrip = listXml.To<T[]>();
            return roundtrip[1];
        }

        public static string ToXml<T>(this T item, bool printXmlToConsole = true)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                using (var writer = new StringWriter(stringBuilder))
                {
                    var type = typeof(T);
                    var serializer = new XmlSerializer(type);
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
            catch (Exception ex)
            {
                Console.WriteLine("#################### Failed writing xml ####################");
                ex.DumpToConsole();
                throw;
            }
        }

        public static T To<T>(this string xml)
        {
            try
            {
                using (var reader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var deserialize = (T)serializer.Deserialize(reader);
                    return deserialize;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("#################### Failed reading xml ####################");
                ex.DumpToConsole();
                throw;
            }
        }

        internal static string Normalize(string xml)
        {
            var e = XElement.Parse(xml);
            return Normalize(e);
        }

        private static string Normalize(XElement e)
        {
            XElement clean = RemoveNamespacesAndSort(e);
            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, Settings))
            {
                clean.WriteTo(writer);
                writer.Flush();
                return sw.ToString();
            }
        }

        private static XElement RemoveNamespacesAndSort(XElement e)
        {
            string content = e.HasElements ? null : e.Value.Trim();
            double d;
            if (double.TryParse(content, NumberStyles.Float, CultureInfo.InvariantCulture, out d))
            {
                content = d.ToString(CultureInfo.InvariantCulture);
            }
            var ne = new XElement(e.Name.LocalName, content);
            ne.Add(
                e.Attributes()
                 .Where(a => !a.IsNamespaceDeclaration)
                 .OrderBy(x => x.Name.LocalName));
            ne.Add(
                e.Elements()
                 .Select(RemoveNamespacesAndSort)
                 .OrderBy(x => x.Name.LocalName)
                 .ThenBy(x => x.Value));
            return ne;
        }
    }
}
