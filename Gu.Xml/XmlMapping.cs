namespace Gu.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Xml;

    public class XmlMapping
    {
        private readonly List<IMap> _attributeMappings = new List<IMap>();
        private readonly List<IMap> _elementMappings = new List<IMap>();

        public IEnumerable<IMap> AttributeMappings
        {
            get
            {
                return _attributeMappings;
            }
        }
        public IEnumerable<IMap> ElementMappings
        {
            get
            {
                return _elementMappings;
            }
        }

        public XmlMapping WithElement<T>(Expression<Func<T>> property)
        {
            _elementMappings.Add(new ElementMap<T>(property));
            return this;
        }

        public XmlMapping WithElement<T>(Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _elementMappings.Add(new ElementMap<T>(property, field));
            return this;
        }

        public XmlMapping WithAttribute<T>(Expression<Func<T>> property)
        {
            _attributeMappings.Add(new AttributeMap<T>(property));
            return this;
        }

        public XmlMapping WithAttribute<T>(Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _elementMappings.Add(new AttributeMap<T>(property, field));
            return this;
        }

        public static XmlMapping GetOrCreate(Func<XmlMapping, XmlMapping> creator)
        {
            return creator(new XmlMapping());
        }

        public interface IMap
        {
            string Name { get; }

            string OwnerName { get; }

            object Value { get; }
            Type Type { get; }

            void SetValue(XmlReader reader);
        }

        public abstract class Map<T> : IMap
        {
            protected Map(Expression<Func<T>> property)
                : this(property.Name(), property, property)
            {
            }

            protected Map(Expression<Func<T>> getter, Expression<Func<T>> setter)
                : this(getter.Name(), getter, setter)
            {
            }

            protected Map(string name, Expression<Func<T>> getter, Expression<Func<T>> setter)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new SerializationException("Element or attribute name cannot be empty");
                }
                if (!setter.CanSet())
                {
                    throw new InvalidOperationException("Cannot set setter");
                }
                Name = name;
                OwnerName = setter.Owner().GetType().Name;
                Getter = getter;
                Setter = setter;
            }

            public Expression<Func<T>> Getter { get; private set; }

            public Expression<Func<T>> Setter { get; private set; }

            public string Name { get; private set; }

            public string OwnerName { get; private set; }

            public object Value
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

            public abstract void SetValue(XmlReader reader);
        }

        public class ElementMap<T> : Map<T>
        {
            internal ElementMap(Expression<Func<T>> property)
                : base(property)
            {
            }

            internal ElementMap(Expression<Func<T>> getter, Expression<Func<T>> setter)
                : base(getter, setter)
            {
            }

            internal ElementMap(string name, Expression<Func<T>> getter, Expression<Func<T>> setter)
                : base(name, getter, setter)
            {
            }

            public override void SetValue(XmlReader reader)
            {
                reader.ReadElement(Name, Setter);
            }
        }

        public class AttributeMap<T> : Map<T>
        {
            internal AttributeMap(Expression<Func<T>> property)
                : base(property)
            {
            }

            internal AttributeMap(Expression<Func<T>> getter, Expression<Func<T>> setter)
                : base(getter, setter)
            {
            }

            internal AttributeMap(string name, Expression<Func<T>> getter, Expression<Func<T>> setter)
                : base(name, getter, setter)
            {
            }

            public override void SetValue(XmlReader reader)
            {
                reader.ReadAttribute(Name, Setter);
            }
        }
    }
}
