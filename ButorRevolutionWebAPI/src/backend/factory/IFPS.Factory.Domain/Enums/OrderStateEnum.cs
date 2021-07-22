namespace IFPS.Factory.Domain.Enums
{
    public enum OrderStateEnum
    {
        None = 0,

        WaitingForFirstPayment = 1,
        FirstPaymentConfirmed = 2,
        UnderMaterialReservation = 3,
        AllMaterialsReserved = 4,
        WaitingForScheduling = 5,
        Scheduled = 6,
        UnderProduction = 7,
        ProductionComplete = 8,
        WaitingForSecondPayment = 9,
        SecondPaymentConfirmed = 10,

        Default = 1000
    }
}
