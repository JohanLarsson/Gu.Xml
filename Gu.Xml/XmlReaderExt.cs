namespace Gu.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    public static partial class XmlReaderExt
    {
        internal static readonly Dictionary<Type, Func<string, object>> FromStrings =
            new Dictionary<Type, Func<string, object>>
                {
                    { typeof(String), x => x },
                    { typeof(Boolean), x => XmlConvert.ToBoolean(x) },
                    { typeof(Char), x => XmlConvert.ToChar(x) },
                    { typeof(Decimal), x => XmlConvert.ToDecimal(x) },
                    { typeof(SByte), x => XmlConvert.ToSByte(x) },
                    { typeof(Int16), x => XmlConvert.ToInt16(x) },
                    { typeof(Int32), x => XmlConvert.ToInt32(x) },
                    { typeof(Int64), x => XmlConvert.ToInt64(x) },
                    { typeof(Byte), x => XmlConvert.ToByte(x) },
                    { typeof(UInt16), x => XmlConvert.ToUInt16(x) },
                    { typeof(UInt32), x => XmlConvert.ToUInt32(x) },
                    { typeof(UInt64), x => XmlConvert.ToUInt64(x) },
                    { typeof(Single), x => XmlConvert.ToSingle(x) },
                    { typeof(Double), x => XmlConvert.ToDouble(x) },
                    { typeof(TimeSpan), x => XmlConvert.ToTimeSpan(x) },
                    { typeof(DateTime), x => XmlConvert.ToDateTime(x, XmlDateTimeSerializationMode.Unspecified) },
                    { typeof(DateTimeOffset), x => XmlConvert.ToDateTimeOffset(x) },
                    { typeof(Guid), x => XmlConvert.ToGuid(x) },
                };

        internal static readonly Dictionary<Type, Func<XmlReader, object>> ReadElementContents =
            new Dictionary<Type, Func<XmlReader, object>>
                {
                    { typeof(System.Object), x => x.ReadElementContentAsObject() },
                    { typeof(System.Boolean), x => x.ReadElementContentAsBoolean() },
                    { typeof(System.DateTime), x => x.ReadElementContentAsDateTime() },
                    { typeof(System.Double), x => x.ReadElementContentAsDouble() },
                    { typeof(System.Single), x => x.ReadElementContentAsFloat() },
                    { typeof(System.Decimal), x => x.ReadElementContentAsDecimal() },
                    { typeof(System.Int32), x => x.ReadElementContentAsInt() },
                    { typeof(System.Int64), x => x.ReadElementContentAsLong() },
                    { typeof(System.String), x => x.ReadElementContentAsString() },
                };

        public static XmlReader ReadAttribute<T>(this XmlReader reader, Expression<Func<T>> property)
        {
            return ReadAttribute(reader, new AttributeMap<T>(property, property, true));
        }

        public static XmlReader ReadAttribute<T>(
            this XmlReader reader,
            Expression<Func<T>> property,
            Expression<Func<T>> field)
        {
            return ReadAttribute(reader, new AttributeMap<T>(property, field, true));
        }

        internal static XmlReader ReadAttribute<T>(this XmlReader reader, AttributeMap<T> map)
        {
            map.Read(reader);
            return reader;
        }

        public static XmlReader ReadElement<T>(this XmlReader reader, Expression<Func<T>> property)
        {
            return ReadElement(reader, new ElementMap<T>(property, property, true));
        }

        public static XmlReader ReadElement<T>(
            this XmlReader reader,
            Expression<Func<T>> property,
            Expression<Func<T>> setter)
        {
            return ReadElement(reader, new ElementMap<T>(property, setter, true));
        }

        internal static XmlReader ReadElement<T>(
            this XmlReader reader,
            ElementMap<T> map)
        {
            map.Read(reader);
            return reader;
        }

        public static void Read<T>(this XmlReader reader, T instance) where T : IXmlMapped
        {
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMappings)
            {
                map.Read(reader);
            }

            foreach (var map in xmlMapping.ElementMappings)
            {
                map.Read(reader);
            }
        }

        public static T ReadAttributeAs<T>(this XmlReader reader, string localName)
        {
            reader.MoveToContent();
            reader.MoveToAttribute(localName);
            var value = reader.ReadAttributeValueAs<T>();
            VerifyNullable<T>(value, localName);
            return value;
        }

        public static T ReadAttributeValueAs<T>(this XmlReader reader)
        {
            if (reader.NodeType != XmlNodeType.Attribute)
            {
                throw new SerializationException("Failing to read attribute reader.NodeType != XmlNodeType.Attribute");
            }
            if (typeof(System.Object) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsObject();
            }
            if (typeof(System.Boolean) == typeof(T) || typeof(System.Nullable<System.Boolean>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsBoolean();
            }
            if (typeof(System.DateTime) == typeof(T) || typeof(System.Nullable<System.DateTime>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDateTime();
            }
            if (typeof(System.DateTimeOffset) == typeof(T) || typeof(System.Nullable<System.DateTimeOffset>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDateTimeOffset();
            }
            if (typeof(System.Double) == typeof(T) || typeof(System.Nullable<System.Double>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDouble();
            }
            if (typeof(System.Single) == typeof(T) || typeof(System.Nullable<System.Single>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsFloat();
            }
            if (typeof(System.Decimal) == typeof(T) || typeof(System.Nullable<System.Decimal>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDecimal();
            }
            if (typeof(System.Int32) == typeof(T) || typeof(System.Nullable<System.Int32>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsInt();
            }
            if (typeof(System.Int64) == typeof(T) || typeof(System.Nullable<System.Int64>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsLong();
            }
            if (typeof(System.String) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsString();
            }
            throw new SerializationException(string.Format("No conversion for {0}", typeof(T).FullName));
        }

        public static T ReadElementAs<T>(this XmlReader reader, string localName, string ownerName)
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement; // (1)
            if (reader.Name == ownerName)
            {
                reader.ReadStartElement();
            }
            if (reader.Name != localName)
            {
                if (!typeof(T).CanBeNull())
                {
                    throw new SerializationException(string.Format("Failed reading element: {0}, could not find it.", localName));
                }
                return default(T);
            }
            if (!isEmptyElement)
            {
                if (reader.CanResolveEntity)
                {
                    var value = reader.ReadElementValueAs<T>();
                    VerifyNullable<T>(value, localName);
                    return value;
                }
                else
                {
                    throw new SerializationException("!reader.CanResolveEntity");
                }
            }
            else
            {
                VerifyNullable<T>(null, localName);
            }
            return default(T);
        }

        public static T ReadElementValueAs<T>(this XmlReader reader)
        {
            if (reader.NodeType != XmlNodeType.Element)
            {
                throw new SerializationException("reader.NodeType != XmlNodeType.Element");
            }
            if (typeof(System.Object) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsObject();
            }
            if (typeof(System.Boolean) == typeof(T) || typeof(System.Nullable<System.Boolean>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsBoolean();
            }
            if (typeof(System.DateTime) == typeof(T) || typeof(System.Nullable<System.DateTime>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsDateTime();
            }
            if (typeof(System.Double) == typeof(T) || typeof(System.Nullable<System.Double>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsDouble();
            }
            if (typeof(System.Single) == typeof(T) || typeof(System.Nullable<System.Single>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsFloat();
            }
            if (typeof(System.Decimal) == typeof(T) || typeof(System.Nullable<System.Decimal>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsDecimal();
            }
            if (typeof(System.Int32) == typeof(T) || typeof(System.Nullable<System.Int32>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsInt();
            }
            if (typeof(System.Int64) == typeof(T) || typeof(System.Nullable<System.Int64>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsLong();
            }
            if (typeof(System.String) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsString();
            }

            throw new SerializationException(string.Format("No conversion for {0}", typeof(T).FullName));
        }

        public static void VerifyNullable<T>(object value, string localName)
        {
            if (value == null && !typeof(T).CanBeNull())
            {
                throw new SerializationException(
                    string.Format(
                        "{0} is null but {1} is not nullable it is of type",
                        localName, typeof(T).Name));
            }
        }
    }
}