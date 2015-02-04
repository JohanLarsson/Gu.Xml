namespace Gu.Xml
{
    using System;

    internal static class TypeExt
    {
        public static bool CanBeNull(this Type type)
        {
            if (type.IsClass)
            {
                return true;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }
            return false;
        }

        public static bool IsNullable(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }
            return false;
        }

        public static Type NullableInnerType(this Type type)
        {
            if (type.IsNullable())
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return underlyingType;
            }
            return null;
        }
    }
}
