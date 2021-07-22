namespace IFPS.Sales.Domain.Enums
{
    public enum OrderStateEnum
    {
        None = 0,

        OrderCreated = 1,
        WaitingForOffer = 2,
        WaitingForOfferFeedback = 3,
        OfferDeclined = 4,
        WaitingForContract = 5,
        WaitingForContractFeedback = 6,
        ContractSigned = 7, // it means WaitingForProduction
        ContractDeclined = 8,
        UnderProduction = 9,
        WaitingForShippingAppointmentReservation = 10,
        WaitingForShipping = 11,
        Delivered = 12,
        WaitingForInstallation = 13,
        Installed = 14,
        WaitingForRepair = 15,
        Completed = 16,
        UnderGuaranteeRepair = 17,
        WaitingForOnSiteSurvey = 18,
        OnSiteSurveyDone = 19,
        WaitingForOnSiteSurveyAppointmentReservation = 20,

        Default = 1000
    }
}
