using System.Linq.Expressions;
using System.Reflection;

namespace UniCore.Infrastructure.Extension
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string? propertyName, bool descending)
        {
            var propName = string.IsNullOrWhiteSpace(propertyName) ? "Id" : propertyName;
            var normalized = propName.Replace("_", "");

            var propInfo = typeof(T).GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, propName, StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(p.Name, normalized, StringComparison.OrdinalIgnoreCase))
                ?? typeof(T).GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null)
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");

            try
            {
                var property = Expression.Property(parameter, propInfo);
                var selector = Expression.Lambda(property, parameter);

                var method = descending ? "OrderByDescending" : "OrderBy";

                var resultExpression = Expression.Call(
                    typeof(Queryable),
                    method,
                    new Type[] { typeof(T), propInfo.PropertyType },
                    source.Expression,
                    Expression.Quote(selector));

                return source.Provider.CreateQuery<T>(resultExpression);
            }
            catch
            {
                return source;
            }
        }

        public static IQueryable<T> ApplyCursorFilter<T>(this IQueryable<T> source, string? cursorToken, string? sortColumn, bool sortDescending)
        {
            if (string.IsNullOrWhiteSpace(cursorToken))
                return source;

            string cursorValue = cursorToken;
            try
            {
                var padded = cursorToken.Trim().Replace('-', '+').Replace('_', '/').Replace(' ', '+');
                switch (padded.Length % 4)
                {
                    case 2: padded += "=="; break;
                    case 3: padded += "="; break;
                }
                var bytes = Convert.FromBase64String(padded);
                var decoded = System.Text.Encoding.UTF8.GetString(bytes);
                if (!string.IsNullOrEmpty(decoded) && !decoded.Any(char.IsControl))
                {
                    cursorValue = decoded;
                }
            }
            catch
            {
                cursorValue = cursorToken;
            }

            var propName = string.IsNullOrWhiteSpace(sortColumn) ? "Id" : sortColumn;
            var normalized = propName.Replace("_", "");

            var propInfo = typeof(T).GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, propName, StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(p.Name, normalized, StringComparison.OrdinalIgnoreCase))
                ?? typeof(T).GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null)
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");

            try
            {
                var property = Expression.Property(parameter, propInfo);
                var targetType = propInfo.PropertyType;

                Expression comparison;

                if (targetType == typeof(string))
                {
                    var compareMethod = typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string) });
                    if (compareMethod == null)
                        return source;

                    var constant = Expression.Constant(cursorValue, typeof(string));
                    var compareCall = Expression.Call(compareMethod, property, constant);
                    var zero = Expression.Constant(0);

                    comparison = sortDescending
                        ? Expression.LessThan(compareCall, zero)
                        : Expression.GreaterThan(compareCall, zero);
                }
                else
                {
                    var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                    object? convertedValue;

                    if (underlyingType == typeof(Guid))
                    {
                        convertedValue = Guid.Parse(cursorValue);
                    }
                    else if (underlyingType == typeof(DateTime))
                    {
                        convertedValue = DateTime.Parse(cursorValue, null, System.Globalization.DateTimeStyles.RoundtripKind);
                    }
                    else if (underlyingType == typeof(DateTimeOffset))
                    {
                        convertedValue = DateTimeOffset.Parse(cursorValue, null, System.Globalization.DateTimeStyles.RoundtripKind);
                    }
                    else
                    {
                        convertedValue = Convert.ChangeType(cursorValue, underlyingType);
                    }

                    Expression constantExpr = Expression.Constant(convertedValue, underlyingType);
                    if (targetType != underlyingType)
                    {
                        constantExpr = Expression.Convert(constantExpr, targetType);
                    }

                    if (underlyingType == typeof(Guid))
                    {
                        var compareMethod = typeof(Guid).GetMethod(nameof(Guid.CompareTo), new[] { typeof(Guid) });
                        if (compareMethod == null)
                            return source;

                        var propForCompare = targetType != underlyingType ? Expression.Property(property, "Value") : property;
                        var compareCall = Expression.Call(propForCompare, compareMethod, Expression.Constant(convertedValue, typeof(Guid)));
                        var zero = Expression.Constant(0);

                        comparison = sortDescending
                            ? Expression.LessThan(compareCall, zero)
                            : Expression.GreaterThan(compareCall, zero);
                    }
                    else
                    {
                        comparison = sortDescending
                            ? Expression.LessThan(property, constantExpr)
                            : Expression.GreaterThan(property, constantExpr);
                    }
                }

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
