using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class OrderedFurnitureUnitSeed : IEntitySeed<OrderedFurnitureUnit>
    {
        public OrderedFurnitureUnit[] Entities => new[]
        {
            // for webshop orders            
            new OrderedFurnitureUnit(new Guid("23cf9320-3062-4db4-aa29-a0594a726f3f")) { Id = 1, FurnitureUnitId = new Guid("a99749e2-d319-4be0-a4ff-d7b159c00f92"), Quantity = 4, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("23cf9320-3062-4db4-aa29-a0594a726f3f")) { Id = 2, FurnitureUnitId = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("23cf9320-3062-4db4-aa29-a0594a726f3f")) { Id = 3, FurnitureUnitId = new Guid("2598bb7c-e2fe-4999-9208-d57c4d0f643b"), Quantity = 5, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("1f8087e2-8134-4801-af76-8276fbf246b7")) { Id = 19, FurnitureUnitId = new Guid("a99749e2-d319-4be0-a4ff-d7b159c00f92"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("1f8087e2-8134-4801-af76-8276fbf246b7")) { Id = 5, FurnitureUnitId = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), Quantity = 2, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("989d1357-1aed-4ee9-9b40-65e494a23784")) { Id = 6, FurnitureUnitId = new Guid("a99749e2-d319-4be0-a4ff-d7b159c00f92"), Quantity = 3, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("989d1357-1aed-4ee9-9b40-65e494a23784")) { Id = 7, FurnitureUnitId = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("989d1357-1aed-4ee9-9b40-65e494a23784")) { Id = 8, FurnitureUnitId = new Guid("44A6A5AC-2C90-49E2-8842-03132DB64231"), Quantity = 2, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("6eaa291b-f125-4f0a-a0b3-5c878d251ba3")) { Id = 9, FurnitureUnitId = new Guid("2598bb7c-e2fe-4999-9208-d57c4d0f643b"), Quantity = 4, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("6eaa291b-f125-4f0a-a0b3-5c878d251ba3")) { Id = 10, FurnitureUnitId = new Guid("44A6A5AC-2C90-49E2-8842-03132DB64231"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("6eaa291b-f125-4f0a-a0b3-5c878d251ba3")) { Id = 11, FurnitureUnitId = new Guid("EB301B12-75D6-4634-BF9C-0AE1BA1B6281"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("6eaa291b-f125-4f0a-a0b3-5c878d251ba3")) { Id = 12, FurnitureUnitId = new Guid("6EACE25E-A36C-4FD7-8341-53BD040ED370"), Quantity = 2, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("98e32021-bcaa-497f-bbf9-e61853ecf295")) { Id = 13, FurnitureUnitId = new Guid("a99749e2-d319-4be0-a4ff-d7b159c00f92"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("98e32021-bcaa-497f-bbf9-e61853ecf295")) { Id = 14, FurnitureUnitId = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("98e32021-bcaa-497f-bbf9-e61853ecf295")) { Id = 15, FurnitureUnitId = new Guid("4105025C-E947-4D82-8E72-216582EC6B94"), Quantity = 1, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("20b33f75-09b7-4bdc-9470-e49db63dc3de")) { Id = 16, FurnitureUnitId = new Guid("2598bb7c-e2fe-4999-9208-d57c4d0f643b"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("20b33f75-09b7-4bdc-9470-e49db63dc3de")) { Id = 17, FurnitureUnitId = new Guid("6EACE25E-A36C-4FD7-8341-53BD040ED370"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("20b33f75-09b7-4bdc-9470-e49db63dc3de")) { Id = 18, FurnitureUnitId = new Guid("061F03F7-B177-4F2A-90BA-AE7BA1B7B0E3"), Quantity = 3, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("1055b58c-c922-4f13-a35c-af0e56083a51")) { Id = 33, FurnitureUnitId = new Guid("a99749e2-d319-4be0-a4ff-d7b159c00f92"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("1055b58c-c922-4f13-a35c-af0e56083a51")) { Id = 20, FurnitureUnitId = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("1055b58c-c922-4f13-a35c-af0e56083a51")) { Id = 21, FurnitureUnitId = new Guid("A968069D-7F53-426D-AAD5-23C085784741"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("1055b58c-c922-4f13-a35c-af0e56083a51")) { Id = 22, FurnitureUnitId = new Guid("6EACE25E-A36C-4FD7-8341-53BD040ED370"), Quantity = 3, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("fd53912a-9a3c-4586-86b1-d704b5fbb180")) { Id = 23, FurnitureUnitId = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("fd53912a-9a3c-4586-86b1-d704b5fbb180")) { Id = 24, FurnitureUnitId = new Guid("D0675634-28E1-424E-BD8D-F5E07DB70263"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("fd53912a-9a3c-4586-86b1-d704b5fbb180")) { Id = 25, FurnitureUnitId = new Guid("061F03F7-B177-4F2A-90BA-AE7BA1B7B0E3"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("fd53912a-9a3c-4586-86b1-d704b5fbb180")) { Id = 26, FurnitureUnitId = new Guid("A968069D-7F53-426D-AAD5-23C085784741"), Quantity = 3, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("52f9338b-263c-4054-81dd-70d6345a171e")) { Id = 27, FurnitureUnitId = new Guid("45e80e7b-1e2e-416a-b9c4-a2d97ffafe03"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("52f9338b-263c-4054-81dd-70d6345a171e")) { Id = 28, FurnitureUnitId = new Guid("6EACE25E-A36C-4FD7-8341-53BD040ED370"), Quantity = 1, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("52f9338b-263c-4054-81dd-70d6345a171e")) { Id = 29, FurnitureUnitId = new Guid("061F03F7-B177-4F2A-90BA-AE7BA1B7B0E3"), Quantity = 1, UnitPrice = null },

            new OrderedFurnitureUnit(new Guid("c654a3f5-3949-4df2-bf6d-ee4a62622368")) { Id = 30, FurnitureUnitId = new Guid("CED5D4AB-0343-4C8A-A0DC-0676D1EAC76C"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("c654a3f5-3949-4df2-bf6d-ee4a62622368")) { Id = 31, FurnitureUnitId = new Guid("45624A3C-1344-41C5-BCF4-20D4C218E1A1"), Quantity = 2, UnitPrice = null },
            new OrderedFurnitureUnit(new Guid("c654a3f5-3949-4df2-bf6d-ee4a62622368")) { Id = 32, FurnitureUnitId = new Guid("D0675634-28E1-424E-BD8D-F5E07DB70263"), Quantity = 1, UnitPrice = null },

            // for offer form (normal order)
            new OrderedFurnitureUnit(new Guid("2418b030-a64b-4724-9702-964cf5eb04c6"), new Guid("ebf87e4c-50b8-402c-b694-ad29183e0bb1"), 7) { Id = 4, UnitPrice = null }
        };
        //public OrderedFurnitureUnit[] Entities => new OrderedFurnitureUnit[] { };
    }
}
