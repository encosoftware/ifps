using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderStateSeed : IEntitySeed<OrderState>
    {
        public OrderState[] Entities => new[]
        {
            new OrderState(OrderStateEnum.WaitingForFirstPayment) { Id = 1 },
            new OrderState(OrderStateEnum.FirstPaymentConfirmed) { Id = 2 },
            new OrderState(OrderStateEnum.UnderMaterialReservation) { Id = 3 },
            new OrderState(OrderStateEnum.AllMaterialsReserved) { Id = 4 },
            new OrderState(OrderStateEnum.WaitingForScheduling) { Id = 5 },
            new OrderState(OrderStateEnum.Scheduled) { Id = 6 },
            new OrderState(OrderStateEnum.UnderProduction) { Id = 7 },
            new OrderState(OrderStateEnum.ProductionComplete) { Id = 8 },
            new OrderState(OrderStateEnum.WaitingForSecondPayment) { Id = 9 },
            new OrderState(OrderStateEnum.SecondPaymentConfirmed) { Id = 10 }
        };
    }
}
