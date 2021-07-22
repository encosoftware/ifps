using ENCO.DDD.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ENCO.DDD.Application.Extensions
{
    public static class OrderingDtoExtensions
    {
        public static void CheckOrderingRequestByMappings(
            this List<OrderingDto> orderings,
            Dictionary<string, string> columnMappings)
        {
            var normalizedKeys = columnMappings.Keys.Select(k => k.ToLower()).ToList();
            if (orderings.Any(o => !normalizedKeys.Contains(o.Column.ToLower())))
            {
                throw new ArgumentOutOfRangeException("There is no ordering defined for one or more properties.");
            }
        }

        public static Func<IQueryable<T>, IOrderedQueryable<T>> ToOrderingExpression<T>(
            this List<OrderingDto> orderings,
            Dictionary<string, string> columnMappings, string defaultColumn)
        {
            Func<IQueryable<T>, IOrderedQueryable<T>> orderExpr = null;

            if (orderings.Count == 0)
            {
                orderExpr = (IQueryable<T> q) => q.OrderByDynamic(defaultColumn, true);
            }
            else
            {
                for (int i = 0; i < orderings.Count; i++)
                {
                    if (i == 0)
                    {
                        var column = columnMappings[orderings[i].Column];
                        var isAscending = !orderings[i].IsDescending;

                        orderExpr = (IQueryable<T> q) => q.OrderByDynamic(column, isAscending);
                    }
                    else
                    {
                        throw new NotSupportedException("Ordering by multiple columns are not supported yet.");
                    }
                }
            }

            return orderExpr;
        }
    }
}
