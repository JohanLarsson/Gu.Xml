namespace Gu.Xml
{
    using System.Xml;

    public interface IMap
    {
        string Name { get; }

        void Read(XmlReader reader);

        void Write(XmlWriter writer);
    }
}