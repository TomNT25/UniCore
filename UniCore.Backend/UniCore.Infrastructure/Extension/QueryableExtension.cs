using System.Linq.Expressions;

namespace UniCore.Infrastructure.Extension
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string? propertyName, bool descending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            // Create the expression: x => x.PropertyName
            var parameter = Expression.Parameter(typeof(T), "x");

            try
            {
                var property = Expression.PropertyOrField(parameter, propertyName);
                var selector = Expression.Lambda(property, parameter);

                var method = descending ? "OrderByDescending" : "OrderBy";

                // Generate the OrderBy/OrderByDescending method call
                var resultExpression = Expression.Call(
                    typeof(Queryable),
                    method,
                    new Type[] { typeof(T), property.Type },
                    source.Expression,
                    Expression.Quote(selector));

                return source.Provider.CreateQuery<T>(resultExpression);
            }
            catch (ArgumentException)
            {
                // If the property doesn't exist on T, just return the original query
                return source;
            }
        }

        public static IQueryable<T> ApplyCursorFilter<T>(this IQueryable<T> source, string? cursorToken, string? sortColumn, bool sortDescending)
        {
            if (string.IsNullOrWhiteSpace(cursorToken))
                return source;

            string cursorValue;
            try
            {
                var bytes = Convert.FromBase64String(cursorToken);
                cursorValue = System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                cursorValue = cursorToken;
            }

            var propName = string.IsNullOrWhiteSpace(sortColumn) ? "Id" : sortColumn;
            var parameter = Expression.Parameter(typeof(T), "x");

            try
            {
                var property = Expression.PropertyOrField(parameter, propName);
                var targetType = property.Type;

                object? convertedValue;
                if (targetType == typeof(string))
                {
                    convertedValue = cursorValue;
                }
                else
                {
                    var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                    convertedValue = Convert.ChangeType(cursorValue, underlyingType);
                }

                var constant = Expression.Constant(convertedValue, targetType);

                Expression comparison = sortDescending
                    ? Expression.LessThan(property, constant)
                    : Expression.GreaterThan(property, constant);

                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                return source.Where(lambda);
            }
            catch
            {
                return source;
            }
        }
    }
}
