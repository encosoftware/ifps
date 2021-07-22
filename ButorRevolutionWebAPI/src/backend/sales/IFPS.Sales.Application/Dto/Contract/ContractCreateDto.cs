using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ContractCreateDto
    {
        public PriceCreateDto FirstPayment { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public PriceCreateDto SecondPayment { get; set; }
        public DateTime SecondPaymentDate { get; set; }
        public string Additional { get; set; }
        public DateTime ContractDate { get; set; }

        public Order UpdateModelObject(Order order)
        {
            order.FirstPayment = new OrderPrice(order.Id, FirstPayment.CreateModelObject(), FirstPaymentDate);
            order.SecondPayment = new OrderPrice(order.Id, SecondPayment.CreateModelObject(), SecondPaymentDate);
            order.Additional = Additional;
            order.ContractDate = ContractDate;
            return order;
        }
    }
}
