using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Gu.Xml.Tests
{
    public struct DummyStruct : IXmlSerializable
    {
        private int _value3;
        private readonly int _value4;

        public DummyStruct(int value3, int value4, int value1, int value2) : this()
        {
            _value3 = value3;
            _value4 = value4;
            Value1 = value1;
            Value2 = value2;
        }
        public int Value1 { get; private set; }
        
        public int Value2 { get; set; }

        public int Value3
        {
            get { return _value3; }
        }

        public int Value4
        {
            get { return _value4; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            //reader.MoveToContent();
            //reader.ReadAttribute(() => Value1);
            //reader.ReadAttribute(() => Value2);
            //reader.ReadAttribute(() => Value3, () => _value3);
            //reader.ReadAttribute(() => Value4, () => _value4);
        }

        public void WriteXml(XmlWriter writer)
        {
            //writer.WriteAttribute(() => Value1);
            //writer.WriteAttribute(() => Value2);
            //writer.WriteAttribute(() => Value3);
            //writer.WriteAttribute(() => Value4);
        }
    }
}
