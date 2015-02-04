namespace Gu.Xml.Tests.CodeGen
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    using NUnit.Framework;

    public class ReadElementContentAsGen
    {
        [Test]
        public void ReadElementContentAs()
        {
            var toStrings = typeof(XmlReader).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(m => m.Name.StartsWith("ReadElementContentAs") &&
                                                        !m.Name.Contains("Async") &&
                                                        !m.GetParameters().Any())
                                             .ToArray();
            foreach (var methodInfo in toStrings)
            {
                Console.WriteLine(@"{{typeof ({0}), x => x.{1}()}},", methodInfo.ReturnType.FullName, methodInfo.Name);
            }
        }
    }
}