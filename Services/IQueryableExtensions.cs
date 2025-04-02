using System.Linq.Expressions;
using System.Reflection;
namespace GraphQLserver.Services
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> SelectDynamic<T>(this IQueryable<T> query, HashSet<string> selectedFields) where T : class, new()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var bindings = new List<MemberBinding>();

            // Always include the primary key (assuming it's named "Id")
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                bindings.Add(Expression.Bind(idProperty, Expression.Property(parameter, idProperty)));
            }

            // Dynamically add only selected fields
            foreach (var field in selectedFields)
            {
                var property = typeof(T).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    bindings.Add(Expression.Bind(property, Expression.Property(parameter, property)));
                }
            }

            // Create a lambda expression: new T { SelectedFields }
            var newExpression = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            var lambda = Expression.Lambda<Func<T, T>>(newExpression, parameter);

            return query.Select(lambda);
        }
    }
}
