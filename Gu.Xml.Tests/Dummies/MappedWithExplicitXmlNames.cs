namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithExplicitXmlNames : IXmlMapped
    {
        private readonly int _value3;
        private readonly int _value4;

        private MappedWithExplicitXmlNames()
        {
        }

        public MappedWithExplicitXmlNames(int value1, int value2, int value3, int value4)
        {
            Value1 = value1;
            Value2 = value2;
            _value3 = value3;
            _value4 = value4;
        }
        public int Value1 { get; set; }

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
            reader.Read(this);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.Write(this);
        }

        public virtual XmlMap GetMap()
        {
            return XmlMap.Create(
                x => x.WithElement("Element1", () => Value1)
                      .WithAttribute("Attribute1", () => Value2)
                      .WithElement("Element3", () => Value3, () => _value3)
                      .WithAttribute("Attribute2", () => Value4, () => _value4));
        }
    }
}
