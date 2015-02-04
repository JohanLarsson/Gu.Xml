namespace Gu.Xml.Tests.Dummies
{
    using System;
    using System.Linq.Expressions;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class MissingAttribute : IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadAttribute("Missing", (Expression<Func<int>>)null);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttribute("Missing", (Expression<Func<int>>)null);
        }
    }
}