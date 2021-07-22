using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class OrderNameListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public OrderNameListDto(Order order)
        {
            Id = order.Id;
            Name = order.OrderName;
        }
    }
}
