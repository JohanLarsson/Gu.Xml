using System;
using System.Linq.Expressions;

namespace Gu.Xml
{
    internal static class NameOf
    {
        public static string Property<T>(Expression<Func<T>> property)
        {
            return ((MemberExpression) property.Body).Member.Name;
        }

        public static string Field<T>(Expression<Func<T>> field)
        {
            return ((MemberExpression)field.Body).Member.Name;
        }
    }
}
