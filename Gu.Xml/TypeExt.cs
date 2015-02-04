namespace Gu.Xml
{
    using System;

    internal static class TypeExt
    {
        public static bool IsNullable(this Type type)
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
    }
}
