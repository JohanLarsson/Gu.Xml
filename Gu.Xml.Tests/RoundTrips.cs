using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class RoundTrips
    {
        [Test]
        public void WithNullableWithNulls()
        {
            var instance = new WithNullable { Value1 = null, Value2 = null };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
        }

        [Test]
        public void WithNullableWithValues()
        {
            var instance = new WithNullable { Value1 = 1, Value2 = 2 };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
        }

        [Test]
        public void MappedWithNullableWithNulls()
        {
            var instance = new MappedWithNullable { Value1 = null, Value2 = null };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
        }

        [Test]
        public void MappedWithNullableWithValues()
        {
            var instance = new MappedWithNullable { Value1 = 1, Value2 = 2 };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
        }

        [Test]
        public void AttributesClass()
        {
            var instance = new AttributesClass(true, "1", 2, 3);
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void MappedAttributesClass()
        {
            var instance = new MappedAttributesClass(true, "1", 2, 3);
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void ElementClass()
        {
            var instance = new ElementClass(true, "1", 2, 3);
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void MappedElementClass()
        {
            var instance =new  MappedElementClass(true, "1", 2, 3);
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void RoundtripXmlMappingDummy()
        {
            var instance = MappedClass.Default;
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void RoundtripXmlClassWithNestedSimpleClass()
        {
            var instance = new ClassWithNestedSimpleClass
                                 {
                                     Value1 = 1,
                                     Value2 = 2,
                                     Value3 = new SimpleClass { Value1 = 1, Value2 = 2 }
                                 };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3.Value1, roundtrip.Value3.Value1);
            Assert.AreEqual(instance.Value3.Value2, roundtrip.Value3.Value2);
        }

        [Test]
        public void MappedWithNestedIXmlSerializable()
        {
            var instance = new MappedWithNestedIXmlSerializable
            {
                Value1 = 1,
                Value2 = 2,
                Value3 = new XmlSerializableClass { Value1 = 1, Value2 = 2 },
                Value4 = 4,
                Value5 = 5
            };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3.Value1, roundtrip.Value3.Value1);
            Assert.AreEqual(instance.Value3.Value2, roundtrip.Value3.Value2);
        }

        [Test]
        public void MappedWithEnumerable()
        {
            var instance = new MappedWithEnumerable(2);
            var roundtrip = instance.Roundtrip();
            CollectionAssert.AreEqual(instance.Items, roundtrip.Items);
        }
    }
}
