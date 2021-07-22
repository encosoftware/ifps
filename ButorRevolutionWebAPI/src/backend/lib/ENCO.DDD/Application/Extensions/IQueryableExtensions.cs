using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ENCO.DDD.Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query, string orderByMember, bool ascending)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                ascending ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(orderBy);
        }
    }
}
