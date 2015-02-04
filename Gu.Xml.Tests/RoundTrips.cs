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
    }
}
