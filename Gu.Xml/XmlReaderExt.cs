namespace Gu.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Xml;

    public static partial class XmlReaderExt
    {
        private static readonly Dictionary<Type, Func<string, object>> Conversions = new Dictionary
            <Type, Func<string, object>>
        {
            {typeof (String), x => x},
            {typeof (Boolean), x => XmlConvert.ToBoolean(x)},
            {typeof (Char), x => XmlConvert.ToChar(x)},
            {typeof (Decimal), x => XmlConvert.ToDecimal(x)},
            {typeof (SByte), x => XmlConvert.ToSByte(x)},
            {typeof (Int16), x => XmlConvert.ToInt16(x)},
            {typeof (Int32), x => XmlConvert.ToInt32(x)},
            {typeof (Int64), x => XmlConvert.ToInt64(x)},
            {typeof (Byte), x => XmlConvert.ToByte(x)},
            {typeof (UInt16), x => XmlConvert.ToUInt16(x)},
            {typeof (UInt32), x => XmlConvert.ToUInt32(x)},
            {typeof (UInt64), x => XmlConvert.ToUInt64(x)},
            {typeof (Single), x => XmlConvert.ToSingle(x)},
            {typeof (Double), x => XmlConvert.ToDouble(x)},
            {typeof (TimeSpan), x => XmlConvert.ToTimeSpan(x)},
            {typeof (DateTime), x => XmlConvert.ToDateTime(x)},
            {typeof (DateTimeOffset), x => XmlConvert.ToDateTimeOffset(x)},
            {typeof (Guid), x => XmlConvert.ToGuid(x)},
        };
        private static readonly Dictionary<Type, Func<XmlReader, object>> ReadElementContents = new Dictionary<Type, Func<XmlReader, object>>
        {
            {typeof (System.Object), x => x.ReadElementContentAsObject()},
            {typeof (System.Boolean), x => x.ReadElementContentAsBoolean()},
            {typeof (System.DateTime), x => x.ReadElementContentAsDateTime()},
            {typeof (System.Double), x => x.ReadElementContentAsDouble()},
            {typeof (System.Single), x => x.ReadElementContentAsFloat()},
            {typeof (System.Decimal), x => x.ReadElementContentAsDecimal()},
            {typeof (System.Int32), x => x.ReadElementContentAsInt()},
            {typeof (System.Int64), x => x.ReadElementContentAsLong()},
            {typeof (System.String), x => x.ReadElementContentAsString()},
        };

        public static XmlReader ReadAttribute<T>(this XmlReader reader, Expression<Func<T>> property)
        {
            return ReadAttribute(reader, property, property);
        }

        public static XmlReader ReadAttribute<T>(this XmlReader reader, Expression<Func<T>> property, Expression<Func<T>> field)
        {
            return ReadAttribute(reader, property.Name(), field);
        }

        public static XmlReader ReadAttribute<T>(this XmlReader reader, string localName, Expression<Func<T>> setter)
        {
            reader.MoveToContent();
            var s = reader.GetAttribute(localName, reader.NamespaceURI);

            if (s != null)
            {
                var func = Conversions[typeof(T)];
                var value = (T)func(s);
                setter.Set(value);
            }
            else
            {
                if (!typeof(T).IsNullable())
                {
                    throw new SerializationException(string.Format("{0} is null but {1} is not nullable it is of type {2}", localName, setter.Name(), typeof(T).Name));
                }
            }

            return reader;
        }

        public static XmlReader ReadElement<T>(this XmlReader reader, Expression<Func<T>> property)
        {
            return ReadElement(reader, property.Name(), property, x => (T)ReadElementContents[typeof(T)](x));
        }

        public static XmlReader ReadElement<T>(this XmlReader reader, Expression<Func<T>> property, Expression<Func<T>> setter)
        {
            return ReadElement(reader, property.Name(), setter, x => (T)ReadElementContents[typeof(T)](x));
        }

        public static XmlReader ReadElement<T>(this XmlReader reader, string name, Expression<Func<T>> setter)
        {
            return ReadElement(reader, name, setter, x => (T)ReadElementContents[typeof(T)](x));
        }

        public static XmlReader ReadElement<T>(this XmlReader reader, string localName, Expression<Func<T>> setter, Func<XmlReader, T> readElementContentAs)
        {
            var ownerName = setter.Owner()
                                 .GetType()
                                 .Name;
            reader.ReadElement(localName, ownerName, typeof(T), x => setter.Set(readElementContentAs(x)));
            return reader;
        }

        internal static XmlReader ReadElement(this XmlReader reader, string localName, string ownerName, Type elementType, Action<XmlReader> setter)
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement; // (1)
            if (reader.Name == ownerName)
            {
                reader.ReadStartElement();
            }
            if (reader.Name != localName)
            {
                if (!elementType.IsNullable())
                {
                    throw new SerializationException(string.Format("Failed reading element: {0}, could not find it.", localName));
                }
                return reader;
            }
            if (!isEmptyElement)
            {
                if (reader.CanResolveEntity)
                {
                    setter(reader);
                }
                else
                {
                    throw new SerializationException("!reader.CanResolveEntity");
                }
            }
            return reader;
        }

        public static void Read<T>(this XmlReader reader, T instance) where T : IXmlMapped
        {
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMappings)
            {
                map.SetValue(reader);
            }

            foreach (var map in xmlMapping.ElementMappings)
            {
                map.SetValue(reader);
                }
        }
    }
}