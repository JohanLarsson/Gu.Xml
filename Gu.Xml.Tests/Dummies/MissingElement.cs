//namespace Gu.Xml.Tests.Dummies
//{
//    using System;
//    using System.Linq.Expressions;
//    using System.Xml;
//    using System.Xml.Schema;
//    using System.Xml.Serialization;

//    public class MissingElement : IXmlSerializable
//    {
//        public XmlSchema GetSchema()
//        {
//            return null;
//        }

//        public void ReadXml(XmlReader reader)
//        {
//            reader.ReadElement("Missing", (Expression<Func<int>>)null, null);
//        }

//        public void WriteXml(XmlWriter writer)
//        {
//            writer.WriteElement("Missing", (Expression<Func<int>>)null);
//        }
//    }
//}