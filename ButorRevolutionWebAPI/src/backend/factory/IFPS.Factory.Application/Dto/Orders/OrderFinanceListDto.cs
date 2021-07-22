using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class OrderFinanceListDto
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public string WorkingNumber { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public DateTime Deadline { get; set; }
        public PriceListDto Amount { get; set; }
        public UserDto Responsible { get; set; }

        public static Func<Order, OrderFinanceListDto> FromEntity = entity => new OrderFinanceListDto
        {
            Id = entity.Id,
            OrderId = entity.OrderName,
            WorkingNumber = entity.WorkingNumber,
            CurrentStatus = entity.CurrentTicket.OrderState.CurrentTranslation.Name,
            StatusDeadline = entity.CurrentTicket.Deadline,
            Deadline = entity.Deadline,
            Responsible = entity.CurrentTicket.AssignedTo != null ? new UserDto(entity.CurrentTicket.AssignedTo) : null,
            Amount = entity.CurrentTicket.OrderState.State == Domain.Enums.OrderStateEnum.WaitingForFirstPayment ?
            new PriceListDto(entity.FirstPayment?.Price) : new PriceListDto(entity.SecondPayment?.Price)
        };       
    }
}
