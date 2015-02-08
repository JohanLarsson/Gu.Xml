namespace Gu.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class XmlMap
    {
        private readonly List<IMap> _attributeMaps = new List<IMap>();
        private readonly List<IMap> _elementMaps = new List<IMap>();

        public IEnumerable<IMap> AttributeMaps
        {
            get
            {
                return _attributeMaps;
            }
        }

        public IEnumerable<IMap> ElementMaps
        {
            get
            {
                return _elementMaps;
            }
        }

        public XmlMap WithElement<T>(Expression<Func<T>> property)
        {
            _elementMaps.Add(new ElementMap<T, T>(property.Name(), property, property, true));
            return this;
        }

        public XmlMap WithElement<T>(string elementName, Expression<Func<T>> property)
        {
            _elementMaps.Add(new ElementMap<T, T>(elementName, property, property, true));
            return this;
        }

        public XmlMap WithElement<TProp, TField>(Expression<Func<TProp>> property, Expression<Func<TField>> field)
            where TField : TProp
        {
            _elementMaps.Add(new ElementMap<TProp, TField>(property.Name(), property, field, true));
            return this;
        }

        public XmlMap WithElement<TProp, TField>(string elementName, Expression<Func<TProp>> property, Expression<Func<TField>> field)
            where TField : TProp
        {
            _elementMaps.Add(new ElementMap<TProp, TField>(elementName, property, field, true));
            return this;
        }

        public XmlMap WithAttribute<T>(Expression<Func<T>> property)
        {
            _attributeMaps.Add(new AttributeMap<T, T>(property.Name(), property, property, true));
            return this;
        }

        public XmlMap WithAttribute<T>(string attributeName, Expression<Func<T>> property)
        {
            _attributeMaps.Add(new AttributeMap<T, T>(attributeName, property, property, true));
            return this;
        }

        public XmlMap WithAttribute<TProp, TField>(Expression<Func<TProp>> property, Expression<Func<TField>> field)
            where TField : TProp
        {
            _attributeMaps.Add(new AttributeMap<TProp, TField>(property.Name(), property, field, true));
            return this;
        }

        public XmlMap WithAttribute<TProp,TField>(string attributeName, Expression<Func<TProp>> property, Expression<Func<TField>> field) 
            where TField : TProp
        {
            _attributeMaps.Add(new AttributeMap<TProp,TField>(attributeName, property, field, true));
            return this;
        }

        public static XmlMap Create(Func<XmlMap, XmlMap> creator)
        {
            return creator(new XmlMap());
        }
    }
}
