namespace Gu.Xml
{
    using System.Xml.Serialization;

    public interface IXmlMapped : IXmlSerializable
    {
        XmlMapping GetMap();
    }
}
