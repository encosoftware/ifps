using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderStateTestSeed : IEntitySeed<OrderState>
    {
        public OrderState[] Entities => new[]
        {
            new OrderState(OrderStateEnum.None) { Id = 10000 },
            new OrderState(OrderStateEnum.WaitingForFirstPayment) { Id = 10001 },
            new OrderState(OrderStateEnum.FirstPaymentConfirmed) { Id = 10002 },
            new OrderState(OrderStateEnum.UnderMaterialReservation) { Id = 10003 },
            new OrderState(OrderStateEnum.AllMaterialsReserved) { Id = 10004 },
            new OrderState(OrderStateEnum.WaitingForScheduling) { Id = 10005 },
            new OrderState(OrderStateEnum.Scheduled) { Id = 10006 },
            new OrderState(OrderStateEnum.UnderProduction) { Id = 10007 },
            new OrderState(OrderStateEnum.ProductionComplete) { Id = 10008 },
            new OrderState(OrderStateEnum.WaitingForSecondPayment) { Id = 10009 },
            new OrderState(OrderStateEnum.SecondPaymentConfirmed) { Id = 10010 }
        };
    }
}
