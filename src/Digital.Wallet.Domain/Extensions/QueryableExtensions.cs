using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Digital.Wallet.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereLikeAny<T>(this IQueryable<T> query, Expression<Func<T, string>> propertySelector, IEnumerable<string> searchTerms)
    {
        var parameter = propertySelector.Parameters[0];
        var property = propertySelector.Body;

        var ilikeMethod = typeof(NpgsqlDbFunctionsExtensions)
            .GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike), [typeof(DbFunctions), typeof(string), typeof(string)]);

        var filteredTerms = searchTerms
            .Where(term => !string.IsNullOrWhiteSpace(term))
            .ToList();

        Expression? orExpression = null;

        foreach (var term in filteredTerms)
        {
            var pattern = $"%{term}%";

            var ilikeCall = Expression.Call(
                ilikeMethod!,
                Expression.Constant(EF.Functions),
                property,
                Expression.Constant(pattern));

            orExpression = orExpression is null
                ? ilikeCall
                : Expression.OrElse(orExpression, ilikeCall);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(orExpression!, parameter);
        return query.Where(lambda);
    }
}