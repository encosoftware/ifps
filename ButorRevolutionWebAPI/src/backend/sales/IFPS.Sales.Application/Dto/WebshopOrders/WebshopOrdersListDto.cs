using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopOrdersListDto
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public DateTime Date { get; set; }
        public PriceListDto SubTotal { get; set; }
        public PriceListDto DelieveryPrice { get; set; }


        public WebshopOrdersListDto(WebshopOrder webshopOrder)
        {
            Id = webshopOrder.Id;
            OrderName = webshopOrder.OrderName;
            Date = webshopOrder.CreationTime.Date;
        }
    }
}
