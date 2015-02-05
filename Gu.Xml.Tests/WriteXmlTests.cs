using System;
using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class WriteXmlTests
    {
        [Test]
        public void WriteElementClass()
        {
            var dummyClass = new ElementClass(true, "", 2, 3);
            var xml = dummyClass.ToXml();
            Console.Write(xml);
        }

        [Test]
        public void WriteClassWithNullable()
        {
            var classWithNullable = new WithNullable();
            var xml = classWithNullable.ToXml();
            Console.WriteLine(xml);
        }
    }
}
