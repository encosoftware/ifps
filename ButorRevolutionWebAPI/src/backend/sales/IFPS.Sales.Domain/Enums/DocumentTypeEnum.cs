namespace IFPS.Sales.Domain.Enums
{
    public enum DocumentTypeEnum
    {
        None = 0,

        FloorPlan = 2,
        EngineeringDrawing = 3,
        Offer = 4, 
        Render = 5, 
        Contract = 6, 
        PaymentRequest = 7, 
        OnSiteMeasurement = 8, 
        ProductionRequest = 9, 
        TechnicalDrawing = 10,
        DeliveryNote = 11, 
        CertificateForInstallment = 12, 
        RepairForm = 13,
        GuaranteeRepairForm = 14,

        Other = 1000
    }
}
