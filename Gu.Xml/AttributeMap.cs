namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Xml;

    public class AttributeMap<T> : Map<T>
    {
        internal AttributeMap(Expression<Func<T>> property, bool verifyReadWrite)
            : base(property, verifyReadWrite)
        {
        }

        internal AttributeMap(Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
            : base(getter, setter, verifyReadWrite)
        {
        }

        internal AttributeMap(string name, Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
            : base(name, getter, setter, verifyReadWrite)
        {
        }

        public override void Read(XmlReader reader)
        {
            base.VerifyRead(reader);
            if (reader.GetAttribute(Name) == null)
            {
                XmlReaderExt.VerifyNullable<T>(null, Name);
                return;
            }
            var value = reader.ReadAttributeAs<T>(Name);
            Setter.Invoke(Owner, value);
        }

        public override void Write(XmlWriter writer)
        {
            writer.WriteAttribute(Name, Value);
        }
    }
}