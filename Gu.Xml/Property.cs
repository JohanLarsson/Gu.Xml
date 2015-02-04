using System.Runtime.Serialization;

namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    internal static class Property
    {
        public static bool CanSet<T>(this Expression<Func<T>> propertyOrField)
        {
            var expression = (MemberExpression)propertyOrField.Body;
            var propertyInfo = expression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                return propertyInfo.SetMethod != null;
            }
            var fieldInfo = expression.Member as FieldInfo;
            return fieldInfo != null;
        }
        public static void Set<T>(this Expression<Func<T>> propertyOrField, T value)
        {
            var expression = (MemberExpression)propertyOrField.Body;
            var owner = propertyOrField.Owner();
            var propertyInfo = expression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                if (propertyInfo.SetMethod == null)
                {
                    throw new SerializationException(string.Format("Failed to set property {0} possible fixes: {1}Supply a field () => _backingField{1}Add a (private) setter {1}Do not serialize the property if it is calculated", propertyOrField.Name(), Environment.NewLine));
                }
                propertyInfo.SetValue(owner, value);
                return;
            }
            var fieldInfo = expression.Member as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(owner, value);
                return;
            }
            throw new SerializationException(string.Format("Failed to set {0}", propertyOrField.Name()));
        }

        public static object Owner<T>(this Expression<Func<T>> propertyOrField)
        {
            var expression = (MemberExpression)propertyOrField.Body;
            var owner = ((ConstantExpression)expression.Expression).Value;
            return owner;
        }

        public static string Name<T>(this Expression<Func<T>> property)
        {
            return NameOf.Property(property);
        }
    }
}
