using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class OrderStateListDto
    {
        public int? Id { get; set; }
        public OrderStateEnum? State { get; set; }
        public string Translation { get; set; }

        public OrderStateListDto() { }

        public OrderStateListDto(OrderState orderState)
        {
            Id = orderState?.Id;
            State = orderState?.State;
            Translation = orderState?.CurrentTranslation.Name;
        }
    }
}
