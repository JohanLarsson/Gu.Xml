namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;

    public class MappedAttributesClass : IXmlMapped
    {
        private int _value3;
        private readonly int _value4;

        private MappedAttributesClass()
        {
        }

        public MappedAttributesClass(bool value1, string value2, int value3, int value4)
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
                x => x.WithAttribute(() => Value1)
                    .WithAttribute(() => Value2)
                    .WithAttribute(() => Value3, () => _value3)
                    .WithAttribute(() => Value4, () => _value4));
        }
    }
}