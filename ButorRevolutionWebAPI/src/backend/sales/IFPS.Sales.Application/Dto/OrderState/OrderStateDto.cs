using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class OrderStateDto
    {
        public int Id { get; set; }
        public OrderStateEnum State { get; set; }
        public string Translation { get; set; }

        public OrderStateDto()
        {

        }

        public OrderStateDto(OrderState state)
        {
            Id = state.Id;
            State = state.State;
            Translation = state.CurrentTranslation.Name;
        }

        public static Expression<Func<OrderState, OrderStateDto>> Projection
        {
            get
            {
                return x => new OrderStateDto
                {
                    Id = x.Id,
                    State = x.State,
                    Translation = x.CurrentTranslation.Name,
                };
            }
        }

        public static OrderStateDto FromEntity(OrderState entity)
        {
            return Projection.Compile().Invoke(entity);
        }
    }
}
