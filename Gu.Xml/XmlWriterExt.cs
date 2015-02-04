using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;

namespace Gu.Xml
{
    using System.Collections.Concurrent;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public static class XmlWriterExt
    {
        private static readonly ConcurrentDictionary<Type, XmlSerializer> Serializers = new ConcurrentDictionary<Type, XmlSerializer>();
        private static readonly Dictionary<Type, Func<object, string>> ToStrings = new Dictionary<Type, Func<object, string>>
        {
            {typeof (System.String), x => (string)x},
            {typeof (System.Boolean), x => XmlConvert.ToString((System.Boolean) x)},
            {typeof (System.Char), x => XmlConvert.ToString((System.Char) x)},
            {typeof (System.Decimal), x => XmlConvert.ToString((System.Decimal) x)},
            {typeof (System.SByte), x => XmlConvert.ToString((System.SByte) x)},
            {typeof (System.Int16), x => XmlConvert.ToString((System.Int16) x)},
            {typeof (System.Int32), x => XmlConvert.ToString((System.Int32) x)},
            {typeof (System.Int64), x => XmlConvert.ToString((System.Int64) x)},
            {typeof (System.Byte), x => XmlConvert.ToString((System.Byte) x)},
            {typeof (System.UInt16), x => XmlConvert.ToString((System.UInt16) x)},
            {typeof (System.UInt32), x => XmlConvert.ToString((System.UInt32) x)},
            {typeof (System.UInt64), x => XmlConvert.ToString((System.UInt64) x)},
            {typeof (System.Single), x => XmlConvert.ToString((System.Single) x)},
            {typeof (System.Double), x => XmlConvert.ToString((System.Double) x)},
            {typeof (System.TimeSpan), x => XmlConvert.ToString((System.TimeSpan) x)},
            {typeof (System.DateTime), x => XmlConvert.ToString((System.DateTime) x, XmlDateTimeSerializationMode.Unspecified)},
            {typeof (System.DateTimeOffset), x => XmlConvert.ToString((System.DateTimeOffset) x)},
            {typeof (System.Guid), x => XmlConvert.ToString((System.Guid) x)},
        };

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, Expression<Func<T>> property, bool verifyReadWrite = false)
        {
            return writer.WriteAttribute(new AttributeMap<T>(property, verifyReadWrite));
        }

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, AttributeMap<T> map)
        {
            map.Write(writer);
            return writer;
        }

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, string localName, T value)
        {
            if (value == null && typeof(T).CanBeNull())
            {
                return writer;
            }
            if (typeof(T) == typeof(string))
            {
                writer.WriteAttribute(localName, value, x => (dynamic)x);
            }
            else
            {
                writer.WriteAttribute(localName, value, x => XmlConvert.ToString((dynamic)x));
            }
            return writer;
        }

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, string localName, T value, Func<T, string> toString)
        {
            if (value == null && typeof(T).CanBeNull())
            {
                return writer;
            }
            string s = null;
            try
            {
                s = toString(value);
            }
            catch (Exception e)
            {

                throw new SerializationException(string.Format("Failed to convert {0} to {1}", value, typeof(T).FullName), e);
            }

            writer.WriteAttributeString(localName, s);
            return writer;
        }

        public static XmlWriter WriteElement<T>(this XmlWriter writer, Expression<Func<T>> property, bool verifyReadWrite = false)
        {
            return writer.WriteElement(new ElementMap<T>(property, verifyReadWrite));
        }

        public static XmlWriter WriteElement<T>(this XmlWriter writer, ElementMap<T> map)
        {
            map.Write(writer);
            return writer;
        }

        public static XmlWriter WriteElement<T>(this XmlWriter writer, string localName, T value)
        {
            if (value == null && typeof(T).CanBeNull())
            {
                return writer;
            }
            if (typeof(T) == typeof(string))
            {
                writer.WriteElementString(localName, (dynamic)value);
                return writer;
            }
            Type type = typeof(T).IsNullable() ? typeof(T).NullableInnerType() : typeof(T);
            Func<object, string> toString;
            if (ToStrings.TryGetValue(type, out toString))
            {
                writer.WriteElementString(localName, XmlConvert.ToString((dynamic)value));
                return writer;
            }
            else
            {
                var serializable = value as IXmlSerializable;
                if (serializable != null)
                {
                    writer.WriteStartElement(localName);
                    serializable.WriteXml(writer);
                    writer.WriteEndElement();
                    return writer;
                }
                throw new NotImplementedException("message");
                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(localName));
                serializer.Serialize(writer, value);
            }
            return writer;
        }

        public static void Write<T>(this XmlWriter writer, T instance) where T : IXmlMapped
        {
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMappings)
            {
                map.Write(writer);
            }

            foreach (var map in xmlMapping.ElementMappings)
            {
                map.Write(writer);
            }
        }
    }
}
