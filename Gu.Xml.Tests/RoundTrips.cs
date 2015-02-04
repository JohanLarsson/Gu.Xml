using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class RoundTrips
    {
        [Test]
        public void RoundtripDummyClass()
        {
            var dummyClass = new AttributesClass(true, "2", 3, 4);
            var roundtrip = dummyClass.Roundtrip();
            Assert.AreEqual(dummyClass.Value1, roundtrip.Value1);
            Assert.AreEqual(dummyClass.Value2, roundtrip.Value2);
            Assert.AreEqual(dummyClass.Value3, roundtrip.Value3);
            Assert.AreEqual(dummyClass.Value4, roundtrip.Value4);
        }
    }
}
