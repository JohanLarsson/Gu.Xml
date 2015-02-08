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
            _elementMaps.Add(new ElementMap<T>(property, true));
            return this;
        }

        public XmlMap WithElement<T>(string elementName, Expression<Func<T>> property)
        {
            _elementMaps.Add(new ElementMap<T>(elementName, property, property, true));
            return this;
        }

        public XmlMap WithElement<T>(Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _elementMaps.Add(new ElementMap<T>(property, field, true));
            return this;
        }

        public XmlMap WithElement<T>(string elementName, Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _elementMaps.Add(new ElementMap<T>(elementName, property, field, true));
            return this;
        }

        public XmlMap WithAttribute<T>(Expression<Func<T>> property)
        {
            _attributeMaps.Add(new AttributeMap<T>(property, true));
            return this;
        }

        public XmlMap WithAttribute<T>(string attributeName, Expression<Func<T>> property)
        {
            _attributeMaps.Add(new AttributeMap<T>(attributeName, property, property, true));
            return this;
        }

        public XmlMap WithAttribute<T>(Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _attributeMaps.Add(new AttributeMap<T>(property, field, true));
            return this;
        }

        public XmlMap WithAttribute<T>(string attributeName, Expression<Func<T>> property, Expression<Func<T>> field)
        {
            _attributeMaps.Add(new AttributeMap<T>(attributeName, property, field, true));
            return this;
        }

        public static XmlMap Create(Func<XmlMap, XmlMap> creator)
        {
            return creator(new XmlMap());
        }
    }
}
