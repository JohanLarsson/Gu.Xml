namespace Gu.Xml
{
    using System.Xml.Serialization;

    /// <summary>
    /// Implement this interface for convenient implementation of IXmlSerializable
    /// </summary>
    public interface IXmlMapped : IXmlSerializable
    {
        /// <summary>
        /// Sample implementation:
        /// 
        ///        public void ReadXml(XmlReader reader)
        ///        {
        ///             reader.Read(this);
        ///        }
        /// 
        ///        public void WriteXml(XmlWriter writer)
        ///        {
        ///              writer.Write(this);
        ///        }
        /// 
        ///        public Gu.Xml.XmlMapping GetMap()
        ///        {
        ///            return Gu.Xml.XmlMapping.GetOrCreate(
        ///                this,
        ///                x => x.WithElement(() => Value1)
        ///                      .WithAttribute(() => Value2)
        ///                      .WithElement(() => Value3, () => _value3) // when the property does not have a set method the field must be specified
        ///                      .WithAttribute(() => Value4, () => _value4)); 
        ///        }
        /// </summary>
        /// <returns></returns>
        XmlMap GetMap();
    }
}
