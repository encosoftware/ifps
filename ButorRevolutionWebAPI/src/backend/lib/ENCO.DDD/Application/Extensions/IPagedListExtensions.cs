using ENCO.DDD.Application.Dto;
using ENCO.DDD.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENCO.DDD.Application.Extensions
{
    public static class IPagedListExtensions
    {
        public static PagedListDto<TResult> ToPagedList<TEntity, TResult>(this IPagedList<TEntity> list, Func<TEntity, TResult> selector)
        {
            return new PagedListDto<TResult>
            {
                PageIndex = list.PageIndex,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount,
                Data = list.Items.Select(selector).ToList()
            };
        }

        public static PagedListDto<TResult> ToPagedList<TResult> (this IPagedList<TResult> list)
        {
            return new PagedListDto<TResult>
            {
                PageIndex = list.PageIndex,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount,
                Data = list.Items
            };
        }
    }
}
