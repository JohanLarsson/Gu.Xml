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
        public void ReadNullableWithValues()
        {
            var classWithNullable = new ClassWithNullable { Value1 = 1, Value2 = 2 };
            var roundtrip = classWithNullable.Roundtrip();
            Assert.AreEqual(classWithNullable.Value1, roundtrip.Value1);
            Assert.AreEqual(classWithNullable.Value2, roundtrip.Value2);
        }


        [Test]
        public void ReadWithMissingMappingThrows()
        {
            string xml = XmlHeader + @"<SimpleIXmlSerializableClass><MissingMapping>Meh</MissingMapping></SimpleIXmlSerializableClass>";
            var exeption = Assert.Throws<InvalidOperationException>(() => { xml.To<SimpleIXmlSerializableClass>(); });
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
