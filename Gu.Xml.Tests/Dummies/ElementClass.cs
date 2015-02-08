using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Gu.Xml.Tests.Dummies
{
    public class ElementClass : IXmlSerializable
    {
        private int _value3;
        private readonly int _value4;

        private ElementClass()
        {
        }

        public ElementClass(bool value1, string value2, int value3, int value4)
        {
            Value1 = value1;
            Value2 = value2;
            _value3 = value3;
            _value4 = value4;
        }

        public bool Value1 { get; private set; }

        public string Value2 { get; set; }

        public int Value3
        {
            get { return _value3; }
        }

        public int Value4
        {
            get { return _value4; }
        }

        public int? Value5 { get; private set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            reader.ReadElement(() => Value1)
                  .ReadElement(() => Value2)
                  .ReadElement(() => Value3, () => _value3)
                  .ReadElement(() => Value4, () => _value4)
                  .ReadElement(() => Value5);
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElement(() => Value1)
                  .WriteElement(() => Value2)
                  .WriteElement(() => Value3)
                  .WriteElement(() => Value4)
                  .WriteElement(() => Value5);
        }
    }
}