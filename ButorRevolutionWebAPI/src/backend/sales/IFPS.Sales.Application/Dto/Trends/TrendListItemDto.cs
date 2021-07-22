using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Application.Dto
{
    public class TrendListItemDto<T>
    {
        public T OrderedItemDto { get; set; }
        public int OrdersCount { get; set; }

        public TrendListItemDto()
        {
                
        }

        public TrendListItemDto(T OrderedItemDto , int OrdersCount)
        {
            this.OrderedItemDto = OrderedItemDto;
            this.OrdersCount = OrdersCount;
        }
    }
}
