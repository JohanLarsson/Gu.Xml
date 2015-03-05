namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;

    public class MappedWithInterfaceProperty : IXmlMapped
    {
        private MappedWithInterfaceProperty()
        {
        }

        public MappedWithInterfaceProperty(IMappedSimpleClass first, IMappedSimpleClass second)
        {
            First = first;
            MappedSimpleClass = second;
        }

        public IMappedSimpleClass First { get; private set; }

        public IMappedSimpleClass MappedSimpleClass { get; private set; }

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
            return new XmlMap().WithElement(() => First)
                               .WithElement(() => MappedSimpleClass);
        }
    }
}