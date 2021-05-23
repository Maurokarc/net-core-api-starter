using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetCoreApi.Toolkit.Extensions
{
    public static class TypeExtension
    {
        private static readonly Type StringType = typeof(string);

        private static readonly IEnumerable<Type> UnsignedNumericList = new List<Type> { typeof(ushort), typeof(uint), typeof(ulong), typeof(byte),
                                                                                         typeof(ushort?), typeof(uint?), typeof(ulong?), typeof(byte?)};

        private static readonly IEnumerable<Type> NumericPointList = new List<Type> { typeof(float), typeof(double), typeof(decimal),
                                                                                      typeof(float?), typeof(double?), typeof(decimal?)};

        private static readonly IEnumerable<Type> NumericTypeList = new List<Type> { typeof(short), typeof(int), typeof(long),
                                                                                     typeof(short?), typeof(int?), typeof(long?)}
                                                                                    .Concat(UnsignedNumericList)
                                                                                    .Concat(NumericPointList);

        private static readonly IEnumerable<Type> DatetimeList = new List<Type> { typeof(DateTime), typeof(DateTime?) };

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static bool IsStringType(this Type type)
        {
            return type != null ? type == StringType : false;
        }

        public static bool IsNumericPointType(this Type type)
        {
            return type != null ? NumericPointList.Any(p => p == type) : false;
        }

        public static bool IsUnsignedType(this Type type)
        {
            return type != null ? UnsignedNumericList.Any(p => p == type) : false;
        }

        public static bool IsNumericType(this Type type)
        {
            return type != null ? NumericTypeList.Any(p => p == type) : false;
        }

        public static bool IsDateTimeType(this Type type)
        {
            return type != null ? DatetimeList.Any(p => p == type) : false;
        }

        public static bool IsGenericIEnumerableType(this Type type)
        {
            return !type.IsStringType() && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}
