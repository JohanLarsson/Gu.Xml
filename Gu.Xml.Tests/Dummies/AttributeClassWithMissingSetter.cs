namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class AttributeClassWithMissingSetter : IXmlSerializable
    {
        private int _value1;

        private AttributeClassWithMissingSetter()
        {
        }

        public AttributeClassWithMissingSetter(int value1)
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
            reader.ReadAttribute(() => Value1);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttribute(() => Value1);
        }
    }
}