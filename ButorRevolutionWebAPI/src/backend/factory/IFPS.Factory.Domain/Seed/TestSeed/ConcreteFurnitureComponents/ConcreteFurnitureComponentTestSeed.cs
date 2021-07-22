using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ConcreteFurnitureComponentTestSeed : IEntitySeed<ConcreteFurnitureComponent>
    {
        public ConcreteFurnitureComponent[] Entities => new[]
        {
            new ConcreteFurnitureComponent(10000,new Guid("b3acef50-88cb-410f-a823-6d6b391611a5")) { Id = 10000, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346")},
            new ConcreteFurnitureComponent(10000,new Guid("6dc5cac9-0339-429f-8302-41269b6192fe")) { Id = 10001 ,QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346")},
            new ConcreteFurnitureComponent(10000, new Guid("6dc5cac9-0339-429f-8302-41269b6192fe")) { Id = 10002, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346")},

            new ConcreteFurnitureComponent(10002, new Guid("4aa9bcaf-63e5-4cd6-aa6c-bf571fefb597"))
            {
                Id = 10003,
                QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346")
            }
        };
    }
}
