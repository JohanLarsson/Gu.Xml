using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
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
            return ReadAttribute(reader, new AttributeMap<T, T>(property.Name(), property, property, true));
        }

        public static XmlReader ReadAttribute<TProp, TField>(
            this XmlReader reader,
            Expression<Func<TProp>> property,
            Expression<Func<TField>> field)
            where TField : TProp
        {
            return ReadAttribute(reader, new AttributeMap<TProp, TField>(property.Name(), property, field, true));
        }

        internal static XmlReader ReadAttribute<TProp, TField>(this XmlReader reader, AttributeMap<TProp, TField> map)
            where TField : TProp
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
            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), reader.ReadContentAsString());
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
            return ReadElement(reader, new ElementMap<T, T>(property.Name(), property, property, true));
        }

        public static XmlReader ReadElement<TProp, TField>(
            this XmlReader reader,
            Expression<Func<TProp>> property,
            Expression<Func<TField>> setter)
            where TField : TProp
        {
            return ReadElement(reader, new ElementMap<TProp, TField>(property.Name(), property, setter, true));
        }

        internal static XmlReader ReadElement<TProp, TField>(
            this XmlReader reader,
            ElementMap<TProp, TField> map)
            where TField : TProp
        {
            map.Read(reader);
            return reader;
        }

        public static T ReadElementAs<T>(this XmlReader reader, string localName)
        {
            var value = reader.ReadElementAs(localName, typeof(T));
            if (value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        public static object ReadElementAs(this XmlReader reader, string localName, Type type)
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement;
            if (reader.Name != localName)
            {
                if (!type.CanBeNull())
                {
                    throw new SerializationException(string.Format("Failed reading element: {0}, could not find it.", localName));
                }
                return null;
            }
            if (!isEmptyElement)
            {
                if (reader.CanResolveEntity)
                {
                    if (type == typeof(string))
                    {
                        return reader.ReadElementContentAsString();
                    }
                    if (type.IsEnum)
                    {
                        return Enum.Parse(type, reader.ReadElementContentAsString());
                    }
                    if (ReadElementContentAsTypes.Contains(type))
                    {
                        var value = reader.ReadElementValueAs(type);
                        VerifyNullable(value, localName, type);
                        return value;
                    }
                    if (typeof(IXmlSerializable).IsAssignableFrom(type))
                    {
                        var instance = (IXmlSerializable)Activator.CreateInstance(type, true);
                        instance.ReadXml(reader);
                        return instance;
                    }
                    else
                    {
                        reader.ReadStartElement();
                        var serializer = new XmlSerializer(type);
                        var value = serializer.Deserialize(reader);
                        reader.ReadEndElement();
                        return value;
                    }
                }
                else
                {
                    throw new SerializationException("!reader.CanResolveEntity");
                }
            }
            else
            {
                VerifyNullable(null, localName, type);
            }
            return null;
        }

        public static T ReadElementValueAs<T>(this XmlReader reader)
        {
            return (T)reader.ReadElementValueAs(typeof(T));
        }

        public static object ReadElementValueAs(this XmlReader reader, Type type)
        {
            if (reader.NodeType != XmlNodeType.Element)
            {
                throw new SerializationException("reader.NodeType != XmlNodeType.Element");
            }

            if (typeof(Boolean) == type || typeof(Nullable<Boolean>) == type)
            {
                return (dynamic)reader.ReadElementContentAsBoolean();
            }
            if (typeof(DateTime) == type || typeof(Nullable<DateTime>) == type)
            {
                return (dynamic)reader.ReadElementContentAsDateTime();
            }
            if (typeof(Double) == type || typeof(Nullable<Double>) == type)
            {
                return (dynamic)reader.ReadElementContentAsDouble();
            }
            if (typeof(Single) == type || typeof(Nullable<Single>) == type)
            {
                return (dynamic)reader.ReadElementContentAsFloat();
            }
            if (typeof(Decimal) == type || typeof(Nullable<Decimal>) == type)
            {
                return (dynamic)reader.ReadElementContentAsDecimal();
            }
            if (typeof(Int32) == type || typeof(Nullable<Int32>) == type)
            {
                return (dynamic)reader.ReadElementContentAsInt();
            }
            if (typeof(Int64) == type || typeof(Nullable<Int64>) == type)
            {
                return (dynamic)reader.ReadElementContentAsLong();
            }

            throw new SerializationException(string.Format("No conversion for {0}", type.FullName));
        }

        public static void Read<T>(this XmlReader reader, T instance) where T : IXmlMapped
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement;
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMaps)
            {
                map.Read(reader);
            }
            reader.Read();
            foreach (var map in xmlMapping.ElementMaps)
            {
                map.Read(reader);
            }
            if (!isEmptyElement)
            {
                reader.ReadEndElement();
            }
        }

        internal static void VerifyNullable<T>(object value, string localName)
        {
            VerifyNullable(value, localName, typeof(T));
        }

        private static void VerifyNullable(object value, string localName, Type type)
        {
            if (value == null && !type.CanBeNull())
            {
                throw new SerializationException(
                    string.Format(
                        "{0} is null but {1} is not nullable it is of type",
                        localName, type.Name));
            }
        }
    }
}