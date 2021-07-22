using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class DocumentGroupSeed : IEntitySeed<DocumentGroup>
    {
        public DocumentGroup[] Entities => new[]
        {
            DocumentGroup.FromSeedData(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),1,true,1),
            DocumentGroup.FromSeedData(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),2,true,2),
            DocumentGroup.FromSeedData(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),3,true,3),
            DocumentGroup.FromSeedData(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),4,false,4),
            DocumentGroup.FromSeedData(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),5,false,5),
            DocumentGroup.FromSeedData(new Guid("2418b030-a64b-4724-9702-964cf5eb04c6"),1,true,6),
            DocumentGroup.FromSeedData(new Guid("2418b030-a64b-4724-9702-964cf5eb04c6"),2,true,7),
            DocumentGroup.FromSeedData(new Guid("2418b030-a64b-4724-9702-964cf5eb04c6"),3,true,8),
            DocumentGroup.FromSeedData(new Guid("2418b030-a64b-4724-9702-964cf5eb04c6"),4,false,9),
            DocumentGroup.FromSeedData(new Guid("2418b030-a64b-4724-9702-964cf5eb04c6"),5,false,10),
        };
        //public DocumentGroup[] Entities => new DocumentGroup[] { };
    }
}
