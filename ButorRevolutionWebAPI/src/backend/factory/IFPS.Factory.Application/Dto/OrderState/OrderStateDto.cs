using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class OrderStateDto
    {
        public int Id { get; set; }
        public OrderStateEnum State { get; set; }
        public string Translation { get; set; }

        public OrderStateDto() { }

        public static Func<OrderState, OrderStateDto> FromModel = model => new OrderStateDto
        {
            Id = model.Id,
            State = model.State,
            Translation = model.CurrentTranslation.Name
        };

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
