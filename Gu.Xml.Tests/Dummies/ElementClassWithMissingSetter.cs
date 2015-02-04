namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class ElementClassWithMissingSetter : IXmlSerializable
    {
        private int _value1;

        private ElementClassWithMissingSetter()
        {
        }

        public ElementClassWithMissingSetter(int value1)
        {
            _value1 = value1;
        }

        public int Value1
        {
            get
            {
                return _value1;
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadElement(() => Value1);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElement(() => Value1);
        }
    }
}