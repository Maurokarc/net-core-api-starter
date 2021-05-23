using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetCoreApi.Infrastructure.Extensions
{
    public static class EFExtensions
    {
        /// <summary>
        /// 使用屬性名稱來進行查找(完全相符)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">屬性名稱</param>
        /// <param name="searchTerm">查找字串</param>
        /// <returns></returns>
        public static IQueryable<T> EqualSearch<T>(this IQueryable<T> source, string propertyName, string searchTerm)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            var item = Expression.Parameter(typeof(T), "item");
            var prop = Expression.Property(item, propertyName);
            var propertyInfo = typeof(T).GetProperty(propertyName);
            var value = Expression.Constant(Convert.ChangeType(searchTerm, propertyInfo.PropertyType));
            BinaryExpression equal = Expression.Equal(prop, value);

            return source.Where(Expression.Lambda<Func<T, bool>>(equal, item));
        }

        /// <summary>
        /// 使用屬性名稱來進行查找(模糊查詢&區分大小寫)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">屬性名稱</param>
        /// <param name="searchTerm">查找字串</param>
        /// <returns></returns>
        public static IQueryable<T> ContainSearch<T>(this IQueryable<T> source, string propertyName, string searchTerm)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            var property = typeof(T).GetProperty(propertyName);

            if (property is null)
            {
                return source;
            }

            searchTerm = "%" + searchTerm + "%";
            var itemParameter = Expression.Parameter(typeof(T), "item");

            var functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions)));
            var like = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { functions.Type, typeof(string), typeof(string) });

            Expression expressionProperty = Expression.Property(itemParameter, property.Name);

            if (property.PropertyType != typeof(string))
            {
                expressionProperty = Expression.Call(expressionProperty, typeof(object).GetMethod(nameof(object.ToString), new Type[0]));
            }

            var selector = Expression.Call(
                       null,
                       like,
                       functions,
                       expressionProperty,
                       Expression.Constant(searchTerm));

            return source.Where(Expression.Lambda<Func<T, bool>>(selector, itemParameter));
        }

        /// <summary>
        /// 同時查找多個欄位(模糊查詢&區分大小寫)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">要查找的屬性集合</param>
        /// <param name="searchTerm">查找字串</param>
        /// <returns></returns>
        public static IQueryable<T> FullSearch<T>(this IQueryable<T> source, IEnumerable<string> propertyNames, string searchTerm)
        {
            if (propertyNames == null || !propertyNames.Any() || string.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            var properties = typeof(T).GetProperties().Where(p => propertyNames.Any(x => x.Equals(p.Name, StringComparison.OrdinalIgnoreCase)));

            searchTerm = $"%{searchTerm}%";
            var itemParameter = Expression.Parameter(typeof(T), "item");

            var functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions)));
            var like = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { functions.Type, typeof(string), typeof(string) });
            Expression selector;

            Expression[] expressions = properties.Select(p =>
            {
                Expression expressionProperty = Expression.Property(itemParameter, p.Name);
                if (p.PropertyType != typeof(string))
                {
                    expressionProperty = Expression.Call(expressionProperty, typeof(object).GetMethod(nameof(object.ToString), new Type[0]));
                }

                return Expression.Call(
                       null,
                       like,
                       functions,
                       expressionProperty,
                       Expression.Constant(searchTerm));

            }).ToArray();

            if (expressions.Length > 1)
            {
                selector = expressions[0];

                for (int i = 1; i < expressions.Length; i++)
                {
                    selector = Expression.OrElse(selector, expressions[i]);
                }

                return source.Where(Expression.Lambda<Func<T, bool>>(selector, itemParameter));
            }
            else
            {
                return source.Where(Expression.Lambda<Func<T, bool>>(expressions[0], itemParameter));
            }
        }

        /// <summary>
        /// 使用屬性名稱來進行排序(ASC)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName">排序的屬性</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            var entityType = typeof(T);

            //Create x=>x.PropName
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var propertyInfo = entityType.GetProperty(property.Member.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            System.Reflection.MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<T>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        /// 使用屬性名稱來進行排序(DESC)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName">排序的屬性</param>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            var entityType = typeof(T);

            //Create x=>x.PropName
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var propertyInfo = entityType.GetProperty(property.Member.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            System.Reflection.MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<T>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
    }
}
