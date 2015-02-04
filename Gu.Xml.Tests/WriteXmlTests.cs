using System;
using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class WriteXmlTests
    {
        [Test]
        public void WriteAttributesClass()
        {
            var dummyClass = AttributesClass.Default;
            var xml = dummyClass.ToXml();
            Console.Write(xml);
        }

        [Test]
        public void WriteElementClass()
        {
            var dummyClass = ElementClass.Default;
            var xml = dummyClass.ToXml();
            Console.Write(xml);
        }

        [Test]
        public void WriteClassWithNullable()
        {
            var classWithNullable = new ClassWithNullable();
            var xml = classWithNullable.ToXml();
            Console.WriteLine(xml);
        }
    }
}
