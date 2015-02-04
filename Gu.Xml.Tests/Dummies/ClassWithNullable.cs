namespace Gu.Xml.Tests.Dummies
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class ClassWithNullable : IXmlSerializable
    {
        public Nullable<int> Value1 { get; set; }

        public string Value2 { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadAttribute(() => Value1);
            reader.ReadElement(() => Value2);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttribute(() => Value1);
            writer.WriteElement(() => Value2);
        }
    }
}
