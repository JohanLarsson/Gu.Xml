namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithEnumerableOfMappedSimpleClass : IXmlMapped
    {
        internal readonly List<MappedSimpleClass> _items = new List<MappedSimpleClass>();

        private MappedWithEnumerableOfMappedSimpleClass()
        {
        }

        public MappedWithEnumerableOfMappedSimpleClass(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _items.Add(new MappedSimpleClass { Value1 = i, Value2 = 2 * i });
            }
        }

        public IEnumerable<MappedSimpleClass> Items
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

    public class MappedWithEnumerableOfInts : IXmlMapped
    {
        internal readonly List<int> _items = new List<int>();

        private MappedWithEnumerableOfInts()
        {
        }

        public MappedWithEnumerableOfInts(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _items.Add(i);
            }
        }

        public IEnumerable<int> Items
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
