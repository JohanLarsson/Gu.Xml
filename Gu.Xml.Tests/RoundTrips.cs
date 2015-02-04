using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class RoundTrips
    {
        [Test]
        public void RoundtripAttributesClass()
        {
            var dummyClass = AttributesClass.Default;
            var roundtrip = dummyClass.Roundtrip();
            Assert.AreEqual(dummyClass.Value1, roundtrip.Value1);
            Assert.AreEqual(dummyClass.Value2, roundtrip.Value2);
            Assert.AreEqual(dummyClass.Value3, roundtrip.Value3);
            Assert.AreEqual(dummyClass.Value4, roundtrip.Value4);
        }

        [Test]
        public void RoundtripElementClass()
        {
            var dummyClass = ElementClass.Default;
            var roundtrip = dummyClass.Roundtrip();
            Assert.AreEqual(dummyClass.Value1, roundtrip.Value1);
            Assert.AreEqual(dummyClass.Value2, roundtrip.Value2);
            Assert.AreEqual(dummyClass.Value3, roundtrip.Value3);
            Assert.AreEqual(dummyClass.Value4, roundtrip.Value4);
        }

        [Test]
        public void RoundtripXmlMappingDummy()
        {
            var dummyClass = MappedClass.Default;
            var roundtrip = dummyClass.Roundtrip();
            Assert.AreEqual(dummyClass.Value1, roundtrip.Value1);
            Assert.AreEqual(dummyClass.Value2, roundtrip.Value2);
            Assert.AreEqual(dummyClass.Value3, roundtrip.Value3);
            Assert.AreEqual(dummyClass.Value4, roundtrip.Value4);
        }

        [Test]
        public void RoundtripXmlClassWithNestedSimpleClass()
        {
            var dummyClass = new ClassWithNestedSimpleClass
                                 {
                                     Value1 = 1,
                                     Value2 = 2,
                                     Value3 = new SimpleClass { Value1 = 1, Value2 = 2 }
                                 };
            var roundtrip = dummyClass.Roundtrip();
            Assert.AreEqual(dummyClass.Value1, roundtrip.Value1);
            Assert.AreEqual(dummyClass.Value2, roundtrip.Value2);
            Assert.AreEqual(dummyClass.Value3.Value1, roundtrip.Value3.Value1);
            Assert.AreEqual(dummyClass.Value3.Value2, roundtrip.Value3.Value2);
        }

        [Test]
        public void RoundtripXmlClassWithNestedSimpleIXmlSerializableClass()
        {
            var dummyClass = new ClassWithNestedSimpleIXmlSerializableClass
            {
                Value1 = 1,
                Value2 = 2,
                Value3 = new SimpleIXmlSerializableClass { Value1 = 1, Value2 = 2 }
            };
            var roundtrip = dummyClass.Roundtrip();
            Assert.AreEqual(dummyClass.Value1, roundtrip.Value1);
            Assert.AreEqual(dummyClass.Value2, roundtrip.Value2);
            Assert.AreEqual(dummyClass.Value3.Value1, roundtrip.Value3.Value1);
            Assert.AreEqual(dummyClass.Value3.Value2, roundtrip.Value3.Value2);
        }

        [Test]
        public void ClassWithEnumerable()
        {
            var instance = new ClassWithEnumerable(2);
            var roundtrip = instance.Roundtrip();
            CollectionAssert.AreEqual(instance.Items, roundtrip.Items);
        }
    }
}
