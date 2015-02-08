namespace Gu.Xml
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlWriterExt
    {
        internal static readonly HashSet<Type> ToStringTypes = new HashSet<Type>
        {
            typeof (System.Boolean),
            typeof (System.Nullable<System.Boolean>),
            typeof (System.Char),
            typeof (System.Nullable<System.Char>),
            typeof (System.Decimal),
            typeof (System.Nullable<System.Decimal>),
            typeof (System.SByte),
            typeof (System.Nullable<System.SByte>),
            typeof (System.Int16),
            typeof (System.Nullable<System.Int16>),
            typeof (System.Int32),
            typeof (System.Nullable<System.Int32>),
            typeof (System.Int64),
            typeof (System.Nullable<System.Int64>),
            typeof (System.Byte),
            typeof (System.Nullable<System.Byte>),
            typeof (System.UInt16),
            typeof (System.Nullable<System.UInt16>),
            typeof (System.UInt32),
            typeof (System.Nullable<System.UInt32>),
            typeof (System.UInt64),
            typeof (System.Nullable<System.UInt64>),
            typeof (System.Single),
            typeof (System.Nullable<System.Single>),
            typeof (System.Double),
            typeof (System.Nullable<System.Double>),
            typeof (System.TimeSpan),
            typeof (System.Nullable<System.TimeSpan>),
            typeof (System.DateTime),
            typeof (System.Nullable<System.DateTime>),
            typeof (System.DateTimeOffset),
            typeof (System.Nullable<System.DateTimeOffset>),
            typeof (System.Guid),
            typeof (System.Nullable<System.Guid>),
        };

        public static XmlWriter WriteAttribute<T>(this XmlWriter writer, Expression<Func<T>> property, bool verifyReadWrite = false)
        {
            return writer.WriteAttribute(new AttributeMap<T, T>(property.Name(), property, property, verifyReadWrite));
        }

        public static XmlWriter WriteAttribute<TProp, TField>(this XmlWriter writer, AttributeMap<TProp, TField> map)
            where TField : TProp
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
            else if (typeof(T).IsEnum)
            {
                writer.WriteAttribute(localName, value, x => (dynamic)x.ToString());
            }
            else if (ToStringTypes.Contains(typeof(T)))
            {
                writer.WriteAttribute(localName, value, x => XmlConvert.ToString((dynamic)x));
            }
            else
            {
                throw new SerializationException("Cannot write {T} as attribute");
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
            return writer.WriteElement(new ElementMap<T, T>(property.Name(), property, property, verifyReadWrite));
        }

        public static XmlWriter WriteElement<TProp, TField>(this XmlWriter writer, ElementMap<TProp, TField> map)
            where TField : TProp
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
            if (typeof(T).IsEnum)
            {
                writer.WriteElementString(localName, (dynamic)value.ToString());
                return writer;
            }
            if (ToStringTypes.Contains(typeof(T)))
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
                else
                {
                    writer.WriteStartElement(localName);
                    var serializer = new XmlSerializer(value.GetType());
                    serializer.Serialize(writer, value);
                    writer.WriteEndElement();
                }
            }
            return writer;
        }

        public static void Write<T>(this XmlWriter writer, T instance) where T : IXmlMapped
        {
            var xmlMapping = instance.GetMap();
            foreach (var map in xmlMapping.AttributeMaps)
            {
                map.Write(writer);
            }

            foreach (var map in xmlMapping.ElementMaps)
            {
                map.Write(writer);
            }
        }
    }
}
