using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace CdIts.CosmosExtensions;

/// <summary>
/// Extensions to allow ordering by a property name instead of ordering by a lambda expression.
/// Original code from https://stackoverflow.com/a/31925185/294804
/// </summary>
public static class OrderByExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property) => ApplyOrder(source, property, "OrderBy");

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property) =>
        ApplyOrder(source, property, "OrderByDescending");

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property) => ApplyOrder(source, property, "ThenBy");

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property) =>
        ApplyOrder(source, property, "ThenByDescending");

    private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
    {
        var props = property.Split('.');
        var type = typeof(T);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (var prop in props)
        {
            // use reflection (not ComponentModel) to mirror LINQ
            // use property name first, then use Newtonsoft.Json.JsonPropertyAttribute to find correct property
            var pi = type.GetProperty(prop) ??
                     type.GetProperties().FirstOrDefault(c => c.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName == prop);
            if (pi is null)
                throw new ArgumentException($"Property {prop} not found in {type}", nameof(property));
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }

        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        var lambda = Expression.Lambda(delegateType, expr, arg);
        var result = typeof(Queryable).GetMethods().Single(method =>
                method.Name == methodName && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 &&
                method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, new object[] { source, lambda });
        return (IOrderedQueryable<T>)result;
    }
}