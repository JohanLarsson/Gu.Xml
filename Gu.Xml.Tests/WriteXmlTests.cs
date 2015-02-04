using System;
using Gu.Xml.Tests.Dummies;
using NUnit.Framework;

namespace Gu.Xml.Tests
{
    public class WriteXmlTests
    {
        [Test]
        public void WriteDummyClass()
        {
            var dummyClass = new AttributesClass(true, "2", 3, 4);
            var xml = dummyClass.ToXml();
            Console.Write(xml);
        }
    }
}
