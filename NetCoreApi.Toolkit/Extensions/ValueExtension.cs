using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreApi.Toolkit.Extensions
{
    public static class ValueExtension
    {
        public static bool IsNull(this object value, bool EmptyStringAsNull = false)
        {
            bool isNull = Convert.IsDBNull(value) || value == null;
            if (!isNull && value.GetType().IsStringType() && EmptyStringAsNull)
            {
                return string.IsNullOrWhiteSpace(Convert.ToString(value));
            }
            else
            {
                return isNull;
            }
        }

        public static bool IgnoreEqual(this string src, string dst)
        {
            return (src ?? string.Empty).Trim().Equals((dst ?? string.Empty).Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        public static List<T> AsList<T>(this IEnumerable<T> source)
        {
            return (source == null || source is List<T>) ? (List<T>)source : source.ToList();
        }

        public static bool RemoveIfExist<T>(this IList<T> source, T target)
        {
            if (source.Contains(target))
            {
                return source.Remove(target);
            }
            else
            {
                return false;
            }
        }

        public static bool RemoveIfExistAt<T>(this IList<T> source, int idx)
        {
            if (idx >= source.Count)
            {
                throw new InvalidOperationException("Index must less than collection count");
            }

            if (idx > -1)
            {
                return source.Remove(source.ElementAt(idx));
            }
            else
            {
                return false;
            }
        }

        public static bool AddIfNotExist<T>(this IList<T> source, T target)
        {
            if (!source.Contains(target))
            {
                source.Add(target);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
