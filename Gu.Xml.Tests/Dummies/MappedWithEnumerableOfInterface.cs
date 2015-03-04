namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithEnumerableOfInterface : IXmlMapped
    {
        internal readonly List<IMappedSimpleClass> _items = new List<IMappedSimpleClass>();

        private MappedWithEnumerableOfInterface()
        {
        }

        public MappedWithEnumerableOfInterface(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _items.Add(new MappedSimpleClass { Value1 = i, Value2 = 2 * i });
            }
        }

        public IEnumerable<IMappedSimpleClass> Items
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