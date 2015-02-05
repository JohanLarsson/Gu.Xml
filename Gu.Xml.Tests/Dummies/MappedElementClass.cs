namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;

    public class MappedElementClass : IXmlMapped
    {
        private int _value3;
        private readonly int _value4;

        private MappedElementClass()
        {
        }

        public MappedElementClass(bool value1, string value2, int value3, int value4)
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
                      .WithElement(() => Value2)
                      .WithElement(() => Value3, () => _value3)
                      .WithElement(() => Value4, () => _value4)
                      .WithElement(() => Value5));
        }
    }
}