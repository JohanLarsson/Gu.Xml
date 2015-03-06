namespace Gu.XmlTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using NUnit.Framework;

    public static class AssertSerialization
    {
        public static T[] RoundtripAll<T>(T item, bool assertAreEqual = true)
        {
            return new[]
                       {
                           BinaryFormatterRoundtrip(item, assertAreEqual),
                           XmlSerializerRoundtrip(item,null, assertAreEqual),
                           DataContractSerializerRoundtrip(item, assertAreEqual),
                       };
        }

        public static T BinaryFormatterRoundtrip<T>(T item, bool assertAreEqual = true)
        {
            return BinaryFormatterRoundtrip(item, assertAreEqual, null);
        }

        /// <summary>
        /// Tests roundtrip with BinaryFormatter.
        /// Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="assertAreEqual">If true Assert.AreEqual(item, roundtripped); is done.
        /// If false no Asserts are invoked</param>
        /// <param name="properties">Additional properties to assert</param>
        /// <returns></returns>
        public static T BinaryFormatterRoundtrip<T>(T item, bool assertAreEqual, params Func<T, object>[] properties)
        {
            try
            {
                var formatter = new BinaryFormatter();
                T roundtripped;
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, item);
                    stream.Position = 0;
                    roundtripped = (T)formatter.Deserialize(stream);
                }
                if (assertAreEqual)
                {
                    AssertProperties.AreEqual(item, roundtripped);
                }
                AssertSpecialProperties(item, roundtripped, properties);
                return roundtripped;
            }
            catch (Exception e)
            {
                e.DumpToConsole();
                throw;
            }
        }

        /// <summary>
        /// Tests roundtrip with XmlSerializer.
        /// Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="expectedXml">The expected xml</param>
        /// <param name="assertAreEqual">If true Assert.AreEqual(item, roundtripped); is done.
        /// If false no Asserts are invoked</param>
        /// <returns></returns>
        public static T XmlSerializerRoundtrip<T>(T item, string expectedXml, bool assertAreEqual = true)
        {
            return XmlSerializerRoundtrip(item, expectedXml, assertAreEqual, null);
        }

        /// <summary>
        /// Tests roundtrip with XmlSerializer.
        /// Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="expectedXml"></param>
        /// <param name="assertAreEqual">If true Assert.AreEqual(item, roundtripped); is done.
        /// If false no Asserts are invoked</param>
        /// <param name="properties">Additional properties to assert</param>
        /// <returns></returns>
        public static T XmlSerializerRoundtrip<T>(T item, string expectedXml, bool assertAreEqual = true, params Func<T, object>[] properties)
        {
            // Using a list in case manual implementation of IXmlSerializable is broken
            try
            {
                var stringBuilder = new StringBuilder();
                var serializer = new XmlSerializer(typeof(T));
                using (var writer = new StringWriter(stringBuilder))
                {
                    serializer.Serialize(writer, item);
                    var actual = stringBuilder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
                    Console.Write(actual);
                    if (expectedXml != null)
                    {
                        Assert.AreEqual(expectedXml.Normalize(), actual.Normalize());
                    }
                }
                stringBuilder = new StringBuilder();
                serializer = new XmlSerializer(typeof(List<T>));
                var list = new List<T> { item, item }; // Testing with list of two to catch bugs in start/end element

                using (var writer = new StringWriter(stringBuilder))
                {
                    serializer.Serialize(writer, list);
                }
                var xml = stringBuilder.ToString();
                T roundtripped;
                using (var reader = new StringReader(xml))
                {
                    var deserialize = serializer.Deserialize(reader);
                    roundtripped = ((List<T>)deserialize)[1];
                }

                if (assertAreEqual)
                {
                    AssertProperties.AreEqual(item, roundtripped);
                }
                AssertSpecialProperties(item, roundtripped, properties);
                return roundtripped;
            }
            catch (Exception e)
            {
                e.DumpToConsole();
                throw;
            }
        }

        /// <summary>
        /// Tests roundtrip with XmlSerializer.
        /// Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="assertAreEqual">If true Assert.AreEqual(item, roundtripped); is done.
        /// If false no Asserts are invoked</param>
        /// <param name="properties">Additional properties to assert</param>
        /// <returns></returns>
        public static T DataContractSerializerRoundtrip<T>(T item, bool assertAreEqual = true, params Func<T, object>[] properties)
        {
            var list = new List<T> { item, item };
            try
            {
                var serializer = new DataContractSerializer(typeof(List<T>));
                var stringBuilder = new StringBuilder();
                using (var writer = XmlWriter.Create(stringBuilder))
                {
                    serializer.WriteObject(writer, list);
                }
                var xml = stringBuilder.ToString();
                Console.Write(xml);
                T roundtripped;
                using (var reader = XmlReader.Create(new StringReader(xml)))
                {
                    var deserialize = serializer.ReadObject(reader);
                    roundtripped = ((List<T>)deserialize)[1];
                }

                if (assertAreEqual)
                {
                    AssertProperties.AreEqual(item, roundtripped);
                }
                AssertSpecialProperties(item, roundtripped, properties);
                return roundtripped;
            }
            catch (Exception e)
            {
                e.DumpToConsole();
                throw;
            }
        }

        private static void AssertSpecialProperties<T>(T instance, T roundtripped, params Func<T, object>[] properties)
        {
            if (properties == null)
            {
                return;
            }
            foreach (var property in properties)
            {
                var expected = property(instance);
                var actual = property(roundtripped);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}