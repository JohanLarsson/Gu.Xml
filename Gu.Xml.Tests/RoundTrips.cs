using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public void MappedClass()
        {
            var instance = new MappedClass(true, "2", 3, 4);
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            CollectionAssert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void ListWithTwoMappedSimples()
        {
            var instance = new List<MappedSimpleClass>
            {
                new MappedSimpleClass {Value1 = 1, Value2 = 2},
                new MappedSimpleClass {Value1 = 3, Value2 = 4}
            };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance[0].Value1, roundtrip[0].Value1);
            Assert.AreEqual(instance[0].Value2, roundtrip[0].Value2);
            Assert.AreEqual(instance[1].Value1, roundtrip[1].Value1);
            Assert.AreEqual(instance[1].Value1, roundtrip[1].Value1);
        }

        [Test]
        public void MappedInherited()
        {
            var instance = new MappedInherited { Value1 = 1, Value2 = 2, Value3 = 3, Value4 = 4 };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }


        [Test]
        public void MappedWithExplicitXmlNames()
        {
            var instance = new MappedWithExplicitXmlNames(1, 2, 3, 4);
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
            var instance = new MappedElementClass(true, "1", 2, 3);
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
            Assert.AreEqual(instance.Value3, roundtrip.Value3);
            Assert.AreEqual(instance.Value4, roundtrip.Value4);
        }

        [Test]
        public void RoundtripXmlMappingDummy()
        {
            var instance = new MappedClass(true, "2", 3, 4);
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
            Assert.AreEqual(instance.Items.Count(), roundtrip.Items.Count());
            for (int i = 0; i < instance._items.Count; i++)
            {
                Assert.AreEqual(instance._items[i].Value1, roundtrip._items[i].Value1);
                Assert.AreEqual(instance._items[i].Value2, roundtrip._items[i].Value2);
            }
        }

        [Test]
        public void MappedWithEnum()
        {
            var instance = new MappedWithEnum
            {
                Value1 = StringComparison.InvariantCulture,
                Value2 = StringComparison.InvariantCultureIgnoreCase
            };
            var roundtrip = instance.Roundtrip();
            Assert.AreEqual(instance.Value1, roundtrip.Value1);
            Assert.AreEqual(instance.Value2, roundtrip.Value2);
        }
    }
}
