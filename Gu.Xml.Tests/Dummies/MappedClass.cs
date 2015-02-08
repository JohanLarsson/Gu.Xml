namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedClass : IXmlMapped
    {
        private int _value3;
        private readonly List<int> _value4;

        private MappedClass()
        {
        }

        public MappedClass(bool value1, string value2, int value3, int value4)
        {
            Value1 = value1;
            Value2 = value2;
            _value3 = value3;
            _value4 = Enumerable.Range(0, value4).ToList();
        }

        public bool Value1 { get; private set; }

        public string Value2 { get; set; }

        public int Value3
        {
            get { return _value3; }
        }

        public IEnumerable<int> Value4
        {
            get { return _value4; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.Read(this);
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.Write(this);
        }

        public virtual XmlMap GetMap()
        {
            return new XmlMap().WithElement(() => Value1)
                               .WithAttribute(() => Value2)
                               .WithAttribute(() => Value3, () => _value3)
                               .WithElement(() => Value4, () => _value4);
        }
    }
}
