using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class OrderStateSeed : IEntitySeed<OrderState>
    {
        public OrderState[] Entities => new[]
        {
            new OrderState(OrderStateEnum.None) { Id = 1 },
            new OrderState(OrderStateEnum.OrderCreated) { Id = 2 },
            new OrderState(OrderStateEnum.WaitingForOffer) { Id = 3 },
            new OrderState(OrderStateEnum.WaitingForOfferFeedback) { Id = 4},
            new OrderState(OrderStateEnum.OfferDeclined) { Id = 5},
            new OrderState(OrderStateEnum.WaitingForContract) { Id = 6},
            new OrderState(OrderStateEnum.ContractSigned) { Id = 7},
            new OrderState(OrderStateEnum.UnderProduction) { Id = 8},
            new OrderState(OrderStateEnum.WaitingForShipping) { Id = 9},
            new OrderState(OrderStateEnum.Delivered) { Id = 10},
            new OrderState(OrderStateEnum.WaitingForInstallation) { Id = 11},
            new OrderState(OrderStateEnum.Installed) { Id = 12},
            new OrderState(OrderStateEnum.WaitingForRepair) { Id = 13},
            new OrderState(OrderStateEnum.Completed) { Id = 14},
            new OrderState(OrderStateEnum.UnderGuaranteeRepair) { Id = 15},
            new OrderState(OrderStateEnum.Default) { Id = 16},

            new OrderState(OrderStateEnum.WaitingForContractFeedback) { Id = 17 },
            new OrderState(OrderStateEnum.ContractDeclined) { Id = 18 },
            new OrderState(OrderStateEnum.WaitingForShippingAppointmentReservation) { Id = 19 },
            new OrderState(OrderStateEnum.WaitingForOnSiteSurvey) { Id = 20 },
            new OrderState(OrderStateEnum.OnSiteSurveyDone) { Id = 21 },
            new OrderState(OrderStateEnum.WaitingForOnSiteSurveyAppointmentReservation) { Id = 22 }

        };
    }
}
