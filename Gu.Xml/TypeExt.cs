namespace Gu.Xml
{
    using System;
    using System.Collections.Generic;

    internal static class TypeExt
    {
        public static bool CanBeNull(this Type type)
        {
            if (type.IsClass)
            {
                return true;
            }
            if (type.IsInterface)
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

        public static bool IsList(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return true;
            }
            return false;
        }

        public static Type ListElementType(this Type listType)
        {
            if (listType == null)
            {
                throw new ArgumentNullException("listType");
            }
            if (!listType.IsList())
            {
                throw new InvalidOperationException(string.Format("Trying to get ListElementType for type {0} that is not List<T>", listType.FullName));
            }

            Type type = (Type)null;
            if (listType.IsGenericType && !listType.IsGenericTypeDefinition
                && object.ReferenceEquals((object)listType.GetGenericTypeDefinition(), (object)typeof(List<>)))
            {
                type = listType.GetGenericArguments()[0];
            }
            return type;
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
