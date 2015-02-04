namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Xml;

    public class ElementMap<T> : Map<T>
    {
        internal ElementMap(Expression<Func<T>> property, bool verifyReadWrite)
            : base(property, verifyReadWrite)
        {
        }

        internal ElementMap(Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
            : base(getter, setter, verifyReadWrite)
        {
        }

        internal ElementMap(string name, Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
            : base(name, getter, setter, verifyReadWrite)
        {
        }

        public override void Read(XmlReader reader)
        {
            base.VerifyRead(reader);
            var value = reader.ReadElementAs<T>(Name, Owner.GetType().Name);
            Setter.Invoke(Owner, value);
        }

        public override void Write(XmlWriter writer)
        {
            writer.WriteElement(Name, Value);
        }
    }
}