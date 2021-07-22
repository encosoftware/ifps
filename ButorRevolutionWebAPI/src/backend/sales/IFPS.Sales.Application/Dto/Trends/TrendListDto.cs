using IFPS.Sales.Domain.Dbos.Trends;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Application.Dto
{
    public class TrendListDto<T>
    {
        public DateTime? IntervalFrom { get; set; }
        public DateTime? IntervalTo { get; set; }
        public int OrdersTotalCount { get; set; }
        public int Take { get; set; }
        public List<TrendListItemDto<T>> Results { get; set; }

        public TrendListDto()
        {

        }
    }
}
