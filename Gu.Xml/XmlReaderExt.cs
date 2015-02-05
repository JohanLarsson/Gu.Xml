using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Xml;
    public static partial class XmlReaderExt
    {
        internal static readonly HashSet<Type> ReadElementContentAsTypes = new HashSet<Type>
        {
            typeof (System.Boolean),
            typeof (System.Nullable<System.Boolean>),
            typeof (System.DateTime),
            typeof (System.Nullable<System.DateTime>),
            typeof (System.DateTimeOffset),
            typeof (System.Nullable<System.DateTimeOffset>),
            typeof (System.Double),
            typeof (System.Nullable<System.Double>),
            typeof (System.Single),
            typeof (System.Nullable<System.Single>),
            typeof (System.Decimal),
            typeof (System.Nullable<System.Decimal>),
            typeof (System.Int32),
            typeof (System.Nullable<System.Int32>),
            typeof (System.Int64),
            typeof (System.Nullable<System.Int64>),
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
            if (typeof(Object) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsObject();
            }
            if (typeof(Boolean) == typeof(T) || typeof(Nullable<Boolean>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsBoolean();
            }
            if (typeof(DateTime) == typeof(T) || typeof(Nullable<DateTime>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDateTime();
            }
            if (typeof(DateTimeOffset) == typeof(T) || typeof(Nullable<DateTimeOffset>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDateTimeOffset();
            }
            if (typeof(Double) == typeof(T) || typeof(Nullable<Double>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDouble();
            }
            if (typeof(Single) == typeof(T) || typeof(Nullable<Single>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsFloat();
            }
            if (typeof(Decimal) == typeof(T) || typeof(Nullable<Decimal>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsDecimal();
            }
            if (typeof(Int32) == typeof(T) || typeof(Nullable<Int32>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsInt();
            }
            if (typeof(Int64) == typeof(T) || typeof(Nullable<Int64>) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsLong();
            }
            if (typeof(String) == typeof(T))
            {
                return (dynamic)reader.ReadContentAsString();
            }
            throw new SerializationException(string.Format("No conversion for {0}", typeof(T).FullName));
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

        public static T ReadElementAs<T>(this XmlReader reader, string localName, string ownerName)
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement;
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
                    if (typeof(T) == typeof(string))
                    {
                        return (dynamic)reader.ReadElementContentAsString();
                    }
                    if (ReadElementContentAsTypes.Contains(typeof(T)))
                    {
                        var value = reader.ReadElementValueAs<T>();
                        VerifyNullable<T>(value, localName);
                        return value;
                    }
                    if (typeof(IXmlSerializable).IsAssignableFrom(typeof(T)))
                    {
                        var instance = (IXmlSerializable)Activator.CreateInstance(typeof(T), true);
                        instance.ReadXml(reader);
                        reader.ReadEndElement();
                        return (T)instance;
                    }
                    throw new NotImplementedException();
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
            if (typeof(Object) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsObject();
            }
            if (typeof(Boolean) == typeof(T) || typeof(Nullable<Boolean>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsBoolean();
            }
            if (typeof(DateTime) == typeof(T) || typeof(Nullable<DateTime>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsDateTime();
            }
            if (typeof(Double) == typeof(T) || typeof(Nullable<Double>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsDouble();
            }
            if (typeof(Single) == typeof(T) || typeof(Nullable<Single>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsFloat();
            }
            if (typeof(Decimal) == typeof(T) || typeof(Nullable<Decimal>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsDecimal();
            }
            if (typeof(Int32) == typeof(T) || typeof(Nullable<Int32>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsInt();
            }
            if (typeof(Int64) == typeof(T) || typeof(Nullable<Int64>) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsLong();
            }
            if (typeof(String) == typeof(T))
            {
                return (dynamic)reader.ReadElementContentAsString();
            }

            throw new SerializationException(string.Format("No conversion for {0}", typeof(T).FullName));
        }

        public static void Read<T>(this XmlReader reader, T instance) where T : IXmlMapped
        {
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMappings)
            {
                map.Read(reader);
            }
            if (xmlMapping.AttributeMappings.Any())
            {
                reader.Read();
            }
            else
            {
                reader.ReadStartElement();
            }

            foreach (var map in xmlMapping.ElementMappings)
            {
                map.Read(reader);
            }
            //reader.ReadEndElement();
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