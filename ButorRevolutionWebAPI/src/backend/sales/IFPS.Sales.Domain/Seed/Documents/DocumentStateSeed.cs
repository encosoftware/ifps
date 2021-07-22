using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Seed.Documents
{
    public class DocumentStateSeed : IEntitySeed<DocumentState>
    {
        public DocumentState[] Entities => new[]
        {
            new DocumentState(DocumentStateEnum.Uploaded) { Id = 1 },
            new DocumentState(DocumentStateEnum.WaitingForApproval) { Id = 2 },
            new DocumentState(DocumentStateEnum.Approved) { Id = 3 },
            new DocumentState(DocumentStateEnum.Declined) { Id = 4 },  
            new DocumentState(DocumentStateEnum.Empty) { Id = 5}
        };
    }
}
