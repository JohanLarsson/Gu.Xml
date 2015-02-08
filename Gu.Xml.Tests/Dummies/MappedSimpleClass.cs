namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;

    public class MappedSimpleClass : IXmlMapped
    {
        public int Value1 { get; set; }
        
        public int Value2 { get; set; }
        
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

        public virtual Gu.Xml.XmlMap GetMap()
        {
            return new XmlMap().WithElement(() => Value1)
                               .WithAttribute(() => Value2);
        }
    }
}