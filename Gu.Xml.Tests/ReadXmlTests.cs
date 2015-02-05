namespace Gu.Xml.Tests
{
    using System;
    using System.Runtime.Serialization;

    using Gu.Xml.Tests.Dummies;
    using NUnit.Framework;

    public class ReadXmlTests
    {
        private const string XmlHeader = @"<?xml version=""1.0"" encoding=""utf-16""?>";
        [Test]
        public void ReadMissingNullable()
        {
            string xml = XmlHeader + @"<WithNullable/>";
            var classWithNullable = xml.To<WithNullable>();
            Assert.IsNull(classWithNullable.Value1);
            Assert.IsNull(classWithNullable.Value2);
        }

        [Test]
        public void ReadWithMissingMappingThrows()
        {
            string xml = XmlHeader + @"<MappedSimpleClass><MissingMapping>Meh</MissingMapping></MappedSimpleClass>";
            var exeption = Assert.Throws<InvalidOperationException>(() => { xml.To<MappedSimpleClass>(); });
            exeption.DumpToConsole();
            Assert.IsInstanceOf<SerializationException>(exeption.InnerException);
        }

        [Test]
        public void ReadAttributeClassWithMissingSetter()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => { new AttributeClassWithMissingSetter(1).Roundtrip(true); });
            exception.DumpToConsole();
            Assert.IsInstanceOf<SerializationException>(exception.InnerException);
        }

        [Test]
        public void ReadElementClassWithMissingSetter()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => { new ElementClassWithMissingSetter(1).Roundtrip(true); });
            exception.DumpToConsole();
            Assert.IsInstanceOf<SerializationException>(exception.InnerException);
        }
    }
}
