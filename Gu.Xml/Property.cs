using System.Runtime.Serialization;

namespace Gu.Xml
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    internal static class Property
    {
        public static void Set<T>(this Expression<Func<T>> propertyOrField, T value)
        {
            var expression = (MemberExpression)propertyOrField.Body;
            var item = ((ConstantExpression)expression.Expression).Value;
            var propertyInfo = expression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(item, value);
                return;
            }
            var fieldInfo = expression.Member as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(item, value);
                return;
            }
            throw new SerializationException(string.Format("Failed to set {0}", propertyOrField.Name()));
        }

        public static string Name<T>(this Expression<Func<T>> property)
        {
            return NameOf.Property(property);
        }
    }
}
