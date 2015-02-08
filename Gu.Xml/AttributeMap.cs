namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Xml;

    public class AttributeMap<TProp, TField> : Map<TProp, TField>
        where TField : TProp
    {
        //internal AttributeMap(Expression<Func<T>> property, bool verifyReadWrite)
        //    : base(property, verifyReadWrite)
        //{
        //}

        //internal AttributeMap(Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
        //    : base(getter, setter, verifyReadWrite)
        //{
        //}

        internal AttributeMap(string name, Expression<Func<TProp>> getter, Expression<Func<TField>> setter, bool verifyReadWrite)
            : base(name, getter, setter, null, null, verifyReadWrite)
        {
        }

        public override void Read(XmlReader reader)
        {
            base.VerifyRead(reader);
            if (reader.GetAttribute(Name) == null)
            {
                XmlReaderExt.VerifyNullable<TField>(null, Name);
                return;
            }
            var value = reader.ReadAttributeAs<TField>(Name);
            Setter.Invoke(Owner, value);
        }

        public override void Write(XmlWriter writer)
        {
            writer.WriteAttribute(Name, Value);
        }
    }
}