namespace Gu.Xml.Tests.Dummies
{
    using System;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithNullable : IXmlMapped
    {
        public Nullable<int> Value1 { get; set; }

        public Nullable<int> Value2 { get; set; }

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
            return XmlMap.Create(
                x => x.WithElement(() => Value1)
                      .WithAttribute(() => Value2));
        }
    }
}