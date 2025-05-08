using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereLikeAny<T>(this IQueryable<T> query, Expression<Func<T, string>> propertySelector, IEnumerable<string> searchTerms)
    {
        var parameter = propertySelector.Parameters[0];
        var property = propertySelector.Body;

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), [typeof(DbFunctions), typeof(string), typeof(string)]);

        var filteredTerms = searchTerms
            .Where(term => !string.IsNullOrEmpty(term))
            .ToList();

        Expression? orExpression = null;
        foreach (var term in filteredTerms)
        {
            var likeExpression = Expression.Call(
                likeMethod!,
                Expression.Constant(EF.Functions),
                property,
                Expression.Constant(StringComparison.OrdinalIgnoreCase));

            orExpression = orExpression ?? Expression.OrElse(left: orExpression!, likeExpression);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(orExpression!, parameter);

        return query.Where(lambda);
    }
}