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
            string xml = XmlHeader + @"<ClassWithNullable/>";
            var classWithNullable = xml.To<ClassWithNullable>();
            Assert.IsNull(classWithNullable.Value1);
            Assert.IsNull(classWithNullable.Value2);
        }

        [Test]
        public void ReadAttributeClassWithMissingSetter()
        {
            string xml = new AttributeClassWithMissingSetter(1).ToXml(true);
            var exception = Assert.Throws<InvalidOperationException>(()=> { xml.To<AttributeClassWithMissingSetter>(); });
            exception.DumpToConsole();
            Assert.IsInstanceOf<SerializationException>(exception.InnerException);
        }

        [Test]
        public void ReadElementClassWithMissingSetter()
        {
            string xml = new ElementClassWithMissingSetter(1).ToXml(true);
            var exception = Assert.Throws<InvalidOperationException>(() => { xml.To<ElementClassWithMissingSetter>(); });
            exception.DumpToConsole();
            Assert.IsInstanceOf<SerializationException>(exception.InnerException);
        }
    }
}
