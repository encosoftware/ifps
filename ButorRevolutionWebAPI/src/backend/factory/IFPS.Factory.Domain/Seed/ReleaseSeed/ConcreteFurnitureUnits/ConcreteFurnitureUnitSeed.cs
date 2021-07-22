using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ConcreteFurnitureUnitSeed : IEntitySeed<ConcreteFurnitureUnit>
    {
        public ConcreteFurnitureUnit[] Entities => new[]
        {
            new ConcreteFurnitureUnit(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51")) { Id = 1, FurnitureUnitId = new Guid("cbcc1b8d-2bf4-4ec6-a411-aa013ed19833") },
            new ConcreteFurnitureUnit(new Guid("0d9f83dc-9143-49e5-9dd7-8dc702f130cb")) { Id = 2, FurnitureUnitId = new Guid("42dfb2f8-c914-48c1-a0b8-cf40806b24db") },
            new ConcreteFurnitureUnit(new Guid("83b6e1cb-215b-42d2-93bb-5dba12fe039e")) { Id = 3, FurnitureUnitId = new Guid("42dfb2f8-c914-48c1-a0b8-cf40806b24db") },
            new ConcreteFurnitureUnit(new Guid("83b6e1cb-215b-42d2-93bb-5dba12fe039e")) { Id = 4, FurnitureUnitId = new Guid("42dfb2f8-c914-48c1-a0b8-cf40806b24db") },
            new ConcreteFurnitureUnit(new Guid("0d9f83dc-9143-49e5-9dd7-8dc702f130cb")) { Id = 5, FurnitureUnitId = new Guid("5799dc7e-f9cb-47a3-8d1c-3cfbc79af794") },

            new ConcreteFurnitureUnit(new Guid("0C60CDBC-FCE3-4833-8FFA-D46664A68DA3")) { Id = 6, FurnitureUnitId = new Guid("51b66111-d87d-457e-ac05-f451e942165f") },
            new ConcreteFurnitureUnit(new Guid("0C60CDBC-FCE3-4833-8FFA-D46664A68DA3")) { Id = 7, FurnitureUnitId = new Guid("8c757afa-2bfa-43a6-94c1-918c19675e64") },
        };
        
        //public ConcreteFurnitureUnit[] Entities => new ConcreteFurnitureUnit[] { };
    }
}
