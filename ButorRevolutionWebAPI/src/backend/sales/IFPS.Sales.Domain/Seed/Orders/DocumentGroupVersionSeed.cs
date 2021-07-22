using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Seed
{
    public class DocumentGroupVersionSeed : IEntitySeed<DocumentGroupVersion>
    {
        public DocumentGroupVersion[] Entities => new[]
        {
            DocumentGroupVersion.FromSeedData(1,5,Clock.Now,null,1),
            DocumentGroupVersion.FromSeedData(1,2,Clock.Now,null,11),
            DocumentGroupVersion.FromSeedData(2,2,Clock.Now,null,2),
            DocumentGroupVersion.FromSeedData(3,2,Clock.Now,null,3),
            DocumentGroupVersion.FromSeedData(4,1,Clock.Now,null,4),
            DocumentGroupVersion.FromSeedData(5,1,Clock.Now,null,5),
            DocumentGroupVersion.FromSeedData(6,5,Clock.Now,null,6),
            DocumentGroupVersion.FromSeedData(7,5,Clock.Now,null,7),
            DocumentGroupVersion.FromSeedData(8,5,Clock.Now,null,8),
            DocumentGroupVersion.FromSeedData(9,5,Clock.Now,null,9),
            DocumentGroupVersion.FromSeedData(10,5,Clock.Now,null,10),
        };
        //public DocumentGroupVersion[] Entities => new DocumentGroupVersion[] { };
    }
}
