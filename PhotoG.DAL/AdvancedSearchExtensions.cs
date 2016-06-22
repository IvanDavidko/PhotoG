using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PhotoG.DAL
{
    public static class AdvancedSearchExtensions
    {
        public static IQueryable<TResult> PopulateConditions<TResult, T>(this IQueryable<TResult> source, T entity)
        {
            IQueryable<TResult> resultExpression = source;
            foreach (var propertyInfo in entity.GetPropertiesWithValues())
            {
                var type = typeof(TResult);
                var value = propertyInfo.GetValue(entity);
                var parameter = Expression.Parameter(type, "p");
                var propertyRef = Expression.Property(parameter, propertyInfo.Name);

                if (propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                {
                    //property is nullable
                    propertyRef = Expression.Property(propertyRef, "Value");
                }

                var constantRef = Expression.Constant(value);
                var expression = Expression.Equal(constantRef, propertyRef);

                resultExpression = resultExpression.Where(Expression.Lambda<Func<TResult, bool>>(expression, new ParameterExpression[] { parameter }));
            }

            return resultExpression;
        }

        private static IEnumerable<PropertyInfo> GetPropertiesWithValues<T>(this T entity)
        {
            var propertyInfos = new List<PropertyInfo>(typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static));
            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(entity);
                if (value == null) continue;

                var type = propertyInfo.PropertyType;

                if (type.IsNullable())
                {
                    // ref-type
                    yield return propertyInfo;
                    continue;
                }

                // value-type
                var exactValue = Convert.ChangeType(value, type);
                if (!exactValue.Equals(Activator.CreateInstance(type)))
                    yield return propertyInfo;
            }
        }

        private static bool IsNullable(this Type type)
        {
            if (!type.IsValueType) return true; // ref-type

            //true is Nullable<T>
            //false is value-type
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
