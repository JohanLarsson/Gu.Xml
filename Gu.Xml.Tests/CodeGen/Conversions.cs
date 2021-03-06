﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NUnit.Framework;

namespace Gu.Xml.Tests.CodeGen
{
    public class Conversions
    {
        [Test]
        public  void ToStringConversion()
        {
            var toStrings = typeof(XmlConvert).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                               .Where(m => m.Name == "ToString")
                                               .ToArray();
            Console.WriteLine("{typeof (System.String), x => (string)x},");
            foreach (var methodInfo in toStrings)
            {
                Console.WriteLine(@"{{typeof ({0}), x => XmlConvert.ToString(({0})x)}},", methodInfo.GetParameters()[0].ParameterType.FullName);
            }
        }

        [Test]
        public void ToStringHashSet()
        {
            var toStrings = typeof(XmlConvert).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                              .Where(m => m.Name == "ToString")
                                              .Select(x=>x.GetParameters()[0].ParameterType)
                                              .Distinct()
                                              .ToArray();
            foreach (var parameterType in toStrings)
            {
                Console.WriteLine(@"typeof({0}),", parameterType.FullName);
                if (parameterType.IsValueType)
                {
                    Console.WriteLine(@"typeof(System.Nullable<{0}>),", parameterType.FullName);
                }
            }
        }

        [Test]
        public void ToX()
        {
            var toStrings = typeof(XmlConvert).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                              .Where(m => m.Name.StartsWith("To") && m.Name != "ToString")
                                              .ToArray();
            Console.WriteLine("{typeof (System.String), x => x},");
            foreach (var methodInfo in toStrings)
            {
                Console.WriteLine(@"{{typeof ({0}), x => XmlConvert.{1}(x)}},", methodInfo.ReturnType.FullName, methodInfo.Name);
            }
        }

        [Test]
        public void ToXHashSet()
        {
            var toStrings = typeof(XmlConvert).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                              .Where(m => m.Name.StartsWith("To") && m.Name != "ToString")
                                              .Select(x => x.GetParameters()[0].ParameterType)
                                              .Distinct()
                                              .ToArray();
            foreach (var parameterType in toStrings)
            {
                Console.WriteLine(@"typeof({0}),", parameterType.FullName);
                if (parameterType.IsValueType)
                {
                    Console.WriteLine(@"typeof(System.Nullable<{0}>),", parameterType.FullName);
                }
            }
        }
    }
}
