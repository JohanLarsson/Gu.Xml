namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;

    public class ClassWithEnumerable : IXmlMapped
    {
        internal readonly List<SimpleIXmlSerializableClass> _items = new List<SimpleIXmlSerializableClass>();

        private ClassWithEnumerable()
        {
        }

        public ClassWithEnumerable(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _items.Add(new SimpleIXmlSerializableClass { Value1 = n, Value2 = 2 * n });
            }
        }
        public IEnumerable<SimpleIXmlSerializableClass> Items
        {
            get
            {
                return _items;
            }
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
            return XmlMap.GetOrCreate(
                this,
                x => x.WithElement(() => Items, () => _items));
        }
    }
}
