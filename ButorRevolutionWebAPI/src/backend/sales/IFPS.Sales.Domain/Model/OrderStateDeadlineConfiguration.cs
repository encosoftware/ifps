namespace IFPS.Sales.Domain.Model
{
    public class OrderStateDeadlineConfiguration
    {
        public int OrderCreated { get; set; }
        public int WaitingForOffer { get; set; }
        public int WaitingForOfferFeedback { get; set; }
        public int OfferDeclined { get; set; }
        public int WaitingForContract { get; set; }
        public int WaitingForContractFeedback { get; set; }
        public int ContractSigned { get; set; }
        public int ContractDeclined { get; set; }
        public int UnderProduction { get; set; }
        public int WaitingForShippingAppointmentReservation { get; set; }
        public int WaitingForShipping { get; set; }
        public int Delivered { get; set; }
        public int WaitingForInstallation { get; set; }
        public int Installed { get; set; }
        public int WaitingForRepair { get; set; }
        public int Completed { get; set; }
        public int UnderGuaranteeRepair { get; set; }
        public int WaitingForOnSiteSurvey { get; set; }
        public int OnSiteSurveyDone { get; set; }
        public int WaitingForOnSiteSurveyAppointmentReservation { get; set; }
    }
}
