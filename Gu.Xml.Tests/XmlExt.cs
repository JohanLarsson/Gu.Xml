namespace Gu.Xml.Tests
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    public static class XmlExt
    {
        public static T Roundtrip<T>(this T item)
        {
            var xml = item.ToXml();
            var roundtrip = xml.To<T>();
            return roundtrip;
        }

        public static string ToXml<T>(this T item)
        {
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, item);
                return stringBuilder.ToString();
            }
        }

        public static T To<T>(this string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                var deserialize =(T) serializer.Deserialize(reader);
                return deserialize;
            }
        }
    }
}
