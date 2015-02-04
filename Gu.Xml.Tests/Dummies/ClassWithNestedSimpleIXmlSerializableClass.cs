﻿namespace Gu.Xml.Tests.Dummies
{
    using System.Xml;
    using System.Xml.Schema;

    public class ClassWithNestedSimpleIXmlSerializableClass : IXmlMapped
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public SimpleIXmlSerializableClass Value3 { get; set; }


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

        public Gu.Xml.XmlMap GetMap()
        {
            return Gu.Xml.XmlMap.GetOrCreate(
                this,
                x => x.WithElement(() => Value1)
                      .WithAttribute(() => Value2)
                      .WithElement(() => Value3));
        }
    }
}