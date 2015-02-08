using System;
using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class WriteXmlTests
    {
        [Test]
        public void MappedElementClass()
        {
            var dummyClass = new MappedElementClass(true, "", 2, 3);
            var xml = dummyClass.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedElementClass>
  <Value1>true</Value1>
  <Value2 />
  <Value3>2</Value3>
  <Value4>3</Value4>
</MappedElementClass>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void MappedElementClassWithNull()
        {
            var dummyClass = new MappedElementClass(true, null, 2, 3);
            var xml = dummyClass.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedElementClass>
  <Value1>true</Value1>
  <Value3>2</Value3>
  <Value4>3</Value4>
</MappedElementClass>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void MappedAttributesClass()
        {
            var dummyClass = new MappedAttributesClass(true, "", 2, 3);
            var xml = dummyClass.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedAttributesClass Value1=""true"" Value2="""" Value3=""2"" Value4=""3"" />";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void MappedAttributesClassWithNull()
        {
            var dummyClass = new MappedAttributesClass(true, null, 2, 3);
            var xml = dummyClass.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedAttributesClass Value1=""true"" Value3=""2"" Value4=""3"" />";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void MappedWithNestedMapped()
        {
            var instance = new MappedWithNestedMapped
            {
                Value1 = 1,
                Value2 = 2,
                Value3 = new MappedSimpleClass { Value1 = 1, Value2 = 2 },
                Value4 = 4,
                Value5 = 5
            };
            var xml = instance.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedWithNestedMapped Value2=""2"">
  <Value1>1</Value1>
  <Value3 Value2=""2"">
    <Value1>1</Value1>
  </Value3>
  <Value4>4</Value4>
  <Value5>5</Value5>
</MappedWithNestedMapped>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void MappedWithExplicitXmlNames()
        {
            var instance = new MappedWithExplicitXmlNames(1, 2, 3, 4);
            var xml = instance.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedWithExplicitXmlNames Attribute1=""2"" Attribute2=""4"">
  <Element1>1</Element1>
  <Element3>3</Element3>
</MappedWithExplicitXmlNames>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void MappedWithNullable()
        {
            var classWithNullable = new MappedWithNullable();
            var xml = classWithNullable.ToXml();
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MappedWithNullable />";
            Assert.AreEqual(expected, xml);
        }
    }
}
