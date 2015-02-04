using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;

namespace Gu.Xml
{
    using System.Runtime.Serialization;

    public static class XmlWriterExt
    {
        private static readonly Dictionary<Type, Func<object, string>> Conversions = new Dictionary<Type, Func<object, string>>
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

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, Expression<Func<T>> property)
        {
            return writer.WriteAttribute(property.Name(), property);
        }

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, string localName, Expression<Func<T>> property)
        {
            var value = property.Compile().Invoke();
            return writer.WriteAttribute(localName, value);
        }

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, string localName, T value)
        {
            if (value == null && typeof(T).IsNullable())
            {
                return writer;
            }
            var func = Conversions[typeof(T)];
            var s = func(value);
            writer.WriteAttributeString(localName, s);
            return writer;
        }

        public static XmlWriter WriteElement<T>(this XmlWriter writer, Expression<Func<T>> property)
        {
            return writer.WriteElement(property.Name(), property);
        }

        public static XmlWriter WriteElement<T>(this XmlWriter writer, string localName, Expression<Func<T>> property)
        {
            if (string.IsNullOrWhiteSpace(localName))
            {
                throw new SerializationException("Element name cannot be blank");
            }
            var value = property.Compile().Invoke();
            return writer.WriteElement(localName, value);
        }

        public static XmlWriter WriteElement<T>(this XmlWriter writer, string localName, T value)
        {
            if (value == null && typeof(T).IsNullable())
            {
                return writer;
            }
            var func = Conversions[typeof(T)];
            var s = func(value);
            writer.WriteElementString(localName, s);
            return writer;
        }

        public static void Write<T>(this XmlWriter writer, T instance) where T : IXmlMapped
        {
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMappings)
            {
                writer.WriteAttribute(map.Name, map.Value);
            }

            foreach (var map in xmlMapping.ElementMappings)
            {
                writer.WriteElement(map.Name, map.Value);
            }
        }
    }
}
