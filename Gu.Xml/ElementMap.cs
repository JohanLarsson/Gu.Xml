namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Xml;

    public class ElementMap<TProp, TField> : Map<TProp, TField>
        where TField : TProp
    {
        internal ElementMap(string name, Expression<Func<TProp>> getter, Expression<Func<TField>> setter, bool verifyReadWrite)
            : base(name, getter, setter, null, null, verifyReadWrite)
        {
        }

        public override void Read(XmlReader reader)
        {
            base.VerifyRead(reader);
            var value = (TField)reader.ReadElementAs(Name, Type);
            Setter.Invoke(Owner, value);
        }

        public override void Write(XmlWriter writer)
        {
            writer.WriteElement(Name, Value);
        }
    }
}