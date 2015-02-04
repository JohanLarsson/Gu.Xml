using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace Gu.Xml
{
    public static class XmlReaderExt
    {
        private static readonly Dictionary<Type, Func<string, object>> Conversions = new Dictionary
            <Type, Func<string, object>>
        {
            {typeof (System.Boolean), x => XmlConvert.ToBoolean(x)},
            {typeof (System.Char), x => XmlConvert.ToChar(x)},
            {typeof (System.Decimal), x => XmlConvert.ToDecimal(x)},
            {typeof (System.SByte), x => XmlConvert.ToSByte(x)},
            {typeof (System.Int16), x => XmlConvert.ToInt16(x)},
            {typeof (System.Int32), x => XmlConvert.ToInt32(x)},
            {typeof (System.Int64), x => XmlConvert.ToInt64(x)},
            {typeof (System.Byte), x => XmlConvert.ToByte(x)},
            {typeof (System.UInt16), x => XmlConvert.ToUInt16(x)},
            {typeof (System.UInt32), x => XmlConvert.ToUInt32(x)},
            {typeof (System.UInt64), x => XmlConvert.ToUInt64(x)},
            {typeof (System.Single), x => XmlConvert.ToSingle(x)},
            {typeof (System.Double), x => XmlConvert.ToDouble(x)},
            {typeof (System.TimeSpan), x => XmlConvert.ToTimeSpan(x)},
            {typeof (System.DateTime), x => XmlConvert.ToDateTime(x)},
            {typeof (System.DateTimeOffset), x => XmlConvert.ToDateTimeOffset(x)},
            {typeof (System.Guid), x => XmlConvert.ToGuid(x)},
        };
        public static XmlReader ReadAttribute<T>(this XmlReader reader, Expression<Func<T>> property)
        {
            var s = reader.GetAttribute(property.Name());
            var func = Conversions[typeof(T)];
            var value = (T)func(s);
            property.Set(value);
            return reader;
        }

        public static XmlReader ReadAttribute<T>(this XmlReader reader, Expression<Func<T>> property, Expression<Func<T>> field)
        {
            var s = reader.GetAttribute(property.Name());
            var func = Conversions[typeof(T)];
            var value = (T)func(s);
            field.Set(value);
            return reader;
        }

        public static int GetAttributeAsInt(this XmlReader reader, string name)
        {
            var value = reader.GetAttribute(name);
            var i = XmlConvert.ToInt32(value);
            return i;
        }
    }
}