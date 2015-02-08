namespace Gu.Xml.Tests.Dummies
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class WithNullable : IXmlSerializable
    {
        public Nullable<int> Value1 { get; set; }

        public Nullable<int> Value2 { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadAttribute(() => Value1);
            reader.Read();
            reader.ReadElement(() => Value2);
            if (Value2 != null)
            {
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttribute(() => Value1);
            writer.WriteElement(() => Value2);
        }
    }
}
