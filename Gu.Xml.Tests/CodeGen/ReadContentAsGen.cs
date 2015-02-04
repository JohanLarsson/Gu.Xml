﻿namespace Gu.Xml.Tests.CodeGen
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    using NUnit.Framework;

    public class ReadContentAsGen
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
                Console.WriteLine(@"if(typeof({0}) == typeof(T) || typeof(System.Nullable<{0}>) == typeof(T))", methodInfo.ReturnType.FullName);
                Console.WriteLine("{");
                Console.WriteLine(@"    return (dynamic)reader.{0}();", methodInfo.Name);
                Console.WriteLine("}");
            }
        }

        [Test]
        public void ReadContentAs()
        {
            var toStrings = typeof(XmlReader).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(m => m.Name.StartsWith("ReadContentAs") &&
                                                        !m.Name.Contains("Async") &&
                                                        !m.GetParameters().Any())
                                             .ToArray();
            foreach (var methodInfo in toStrings)
            {
                Console.WriteLine(@"if(typeof({0}) == typeof(T) || typeof(System.Nullable<{0}>) == typeof(T))", methodInfo.ReturnType.FullName);
                Console.WriteLine("{");
                Console.WriteLine(@"    return (dynamic)reader.{0}();",methodInfo.Name);
                Console.WriteLine("}");
            }
        }
    }
}