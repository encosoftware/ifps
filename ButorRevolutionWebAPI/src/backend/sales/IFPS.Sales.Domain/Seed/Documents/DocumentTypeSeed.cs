using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Documents
{
    public class DocumentTypeSeed : IEntitySeed<DocumentType>
    {
        public DocumentType[] Entities => new[]
        {
            new DocumentType(DocumentTypeEnum.Render, 1) { Id = 1 },
            new DocumentType(DocumentTypeEnum.Offer, 2) { Id = 2 },
            new DocumentType(DocumentTypeEnum.Contract, 3) { Id = 3 },
            new DocumentType(DocumentTypeEnum.EngineeringDrawing, 4) { Id = 4 },
            new DocumentType(DocumentTypeEnum.TechnicalDrawing, 4) { Id = 5 },
            new DocumentType(DocumentTypeEnum.FloorPlan, 4) { Id = 6 },
            new DocumentType(DocumentTypeEnum.OnSiteMeasurement, 6) { Id = 7 },
            new DocumentType(DocumentTypeEnum.PaymentRequest, 5) { Id = 8 },
            new DocumentType(DocumentTypeEnum.ProductionRequest, 5) { Id = 9 },
            new DocumentType(DocumentTypeEnum.RepairForm, 5) { Id = 10 },
            new DocumentType(DocumentTypeEnum.DeliveryNote, 6) { Id = 11 },
            new DocumentType(DocumentTypeEnum.CertificateForInstallment, 6) { Id = 12 },
            new DocumentType(DocumentTypeEnum.GuaranteeRepairForm, 5) { Id = 13 },

        };
    }
}
