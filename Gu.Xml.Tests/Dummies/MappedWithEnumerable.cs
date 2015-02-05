namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithEnumerable : IXmlMapped
    {
        internal readonly List<XmlSerializableClass> _items = new List<XmlSerializableClass>();

        private MappedWithEnumerable()
        {
        }

        public MappedWithEnumerable(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _items.Add(new XmlSerializableClass { Value1 = n, Value2 = 2 * n });
            }
        }
        public IEnumerable<XmlSerializableClass> Items
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
            return XmlMap.Create(x => x.WithElement(() => Items, () => _items));
        }
    }
}
