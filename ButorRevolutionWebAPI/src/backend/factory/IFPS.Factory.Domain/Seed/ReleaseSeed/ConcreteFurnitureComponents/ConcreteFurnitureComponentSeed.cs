using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ConcreteFurnitureComponentSeed : IEntitySeed<ConcreteFurnitureComponent>
    {
        public ConcreteFurnitureComponent[] Entities => new[]
        {
            new ConcreteFurnitureComponent(1,new Guid("b17bfd96-adec-4e40-92ec-89f874499204")) { Id = 1, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(1,new Guid("efbb21c9-39de-417e-8de4-3453d8fc3c1c")) { Id = 2, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(3,new Guid("cd44769e-7d10-4b72-8513-fcec1bba447a")) { Id = 3, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(4,new Guid("8f092adb-cf14-4d7f-999c-62c68a1d9e87")) { Id = 4, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(2,new Guid("8f092adb-cf14-4d7f-999c-62c68a1d9e87")) { Id = 5, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(5,new Guid("8f092adb-cf14-4d7f-999c-62c68a1d9e87")) { Id = 6, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },

            new ConcreteFurnitureComponent(6,new Guid("6696676b-12d0-4511-b08f-b7d820d2d395")) { Id = 7, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(6,new Guid("dd9f3d57-bf6f-4de1-b5a9-43f0f581f693")) { Id = 8, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(6,new Guid("72171d05-4b62-41c8-ad47-e45e606a0da8")) { Id = 9, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(6,new Guid("225ca25c-b36a-42f7-bed8-964e4f7623ee")) { Id = 10, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(7,new Guid("9e0bfab1-3642-46d8-94ac-1494e4416b11")) { Id = 11, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(7,new Guid("3d3a82bc-b660-4d55-9bd6-314c08eb0dad")) { Id = 12, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
            new ConcreteFurnitureComponent(7,new Guid("c63f05f0-cb6b-480c-a4f1-71ca9d3aa7f5")) { Id = 13, QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346") },
        };
        
        //public ConcreteFurnitureComponent[] Entities => new ConcreteFurnitureComponent[] { };
    }
}
