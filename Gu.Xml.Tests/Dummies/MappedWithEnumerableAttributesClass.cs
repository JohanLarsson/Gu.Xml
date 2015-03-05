namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithEnumerableAttributesClass : IXmlMapped
    {
        internal readonly List<AttributesClass> _items = new List<AttributesClass>();

        private MappedWithEnumerableAttributesClass()
        {
        }

        public MappedWithEnumerableAttributesClass(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _items.Add(new AttributesClass(i % 2 == 0, i.ToString(), i, i * 2));
            }
        }

        public IEnumerable<AttributesClass> Items
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
            return new XmlMap().WithElement(() => Items, () => _items);
        }
    }
}