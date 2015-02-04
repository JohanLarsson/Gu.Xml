namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Xml;

    public abstract class Map<T> : IMap
    {
        protected Map(Expression<Func<T>> property, bool verifyReadWrite)
            : this(property.Name(), property, property, verifyReadWrite)
        {
        }

        protected Map(Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
            : this(getter.Name(), getter, setter, verifyReadWrite)
        {
        }

        protected Map(string name, Expression<Func<T>> getter, Expression<Func<T>> setter, bool verifyReadWrite)
            : this(name, getter, setter, null, null, verifyReadWrite)
        {
        }

        protected Map(string name, Expression<Func<T>> getter, Expression<Func<T>> setter, Action<XmlReader, T> serialize, Func<XmlReader, T> deserialize, bool verifyReadWrite)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SerializationException("Element or attribute name cannot be empty");
            }
            if (verifyReadWrite && !setter.CanSet())
            {
                ThrowCannotSetSetter();
            }
            Name = name;
            Owner = setter.Owner();
            Getter = getter;
            Serialize = serialize;
            Deserialize = deserialize;
            Setter = setter.Setter();
            CanSet = setter.CanSet();
        }

        public Expression<Func<T>> Getter { get; private set; }
        
        public Action<XmlReader, T> Serialize { get; private set; }
        
        public Func<XmlReader, T> Deserialize { get; private set; }

        public Action<object, T> Setter { get; private set; }

        public string Name { get; private set; }

        public object Owner { get; private set; }

        public bool CanSet { get; private set; }

        public T Value
        {
            get
            {
                return Getter.Compile()
                             .Invoke();
            }
        }

        public Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        public abstract void Read(XmlReader reader);

        public abstract void Write(XmlWriter writer);

        protected void VerifyRead(XmlReader reader)
        {
            if (!CanSet)
            {
                ThrowCannotSetSetter();
            }
        }

        private void ThrowCannotSetSetter()
        {
            throw new SerializationException(string.Format("Failng to set value for element: {0} read from xml. Possible solutions: {1} -1 Add (private) set; to the property {1} -2 Supply a field mapping ex: () => _backingField{1} -3 Do not serialize if it is a calculated property", Name, Environment.NewLine));
        }
    }
}