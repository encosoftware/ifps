using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Documents
{
    public class DocumentFolderSeed : IEntitySeed<DocumentFolder>
    {
        public DocumentFolder[] Entities => new[]
        {
            new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, true) { Id = 1 },
            new DocumentFolder(DocumentFolderTypeEnum.QuotationDocument, true) { Id = 2 },
            new DocumentFolder(DocumentFolderTypeEnum.ContractDocument, true) { Id = 3 },
            new DocumentFolder(DocumentFolderTypeEnum.CustomerDocument, false) { Id = 4 },
            new DocumentFolder(DocumentFolderTypeEnum.ProductionDocument, false) { Id = 5 },
            new DocumentFolder(DocumentFolderTypeEnum.PartnerDocument, false) { Id = 6 },
        };
    }
}
