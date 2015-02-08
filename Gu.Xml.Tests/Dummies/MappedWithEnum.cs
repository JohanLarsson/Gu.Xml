namespace Gu.Xml.Tests.Dummies
{
    using System;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithEnum : IXmlMapped
    {
        public MappedWithEnum()
        {
        }

        public StringComparison Value1 { get; set; }
        public StringComparison Value2 { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read(this);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.Write(this);
        }

        public XmlMap GetMap()
        {
            return new XmlMap().WithElement(() => Value1).WithAttribute(() => Value2);
        }
    }
}
