namespace Gu.Xml
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class XmlMap
    {
        private static readonly ConcurrentDictionary<Type, XmlMap> Cache = new ConcurrentDictionary<Type, XmlMap>();
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

        public XmlMap WithElement<T>(Expression<Func<T>> property)
        {
            _elementMappings.Add(new ElementMap<T>(property, true));
            return this;
        }

        public XmlMap WithElement<T>(Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _elementMappings.Add(new ElementMap<T>(property, field, true));
            return this;
        }

        public XmlMap WithAttribute<T>(Expression<Func<T>> property)
        {
            _attributeMappings.Add(new AttributeMap<T>(property, true));
            return this;
        }

        public XmlMap WithAttribute<T>(Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _elementMappings.Add(new AttributeMap<T>(property, field, true));
            return this;
        }

        public static XmlMap GetOrCreate<T>(T instance, Func<XmlMap, XmlMap> creator)
        {
            return Cache.GetOrAdd(
                instance.GetType(),
                creator(new XmlMap()));
        }
    }
}
