using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class FurnitureComponentSeed : IEntitySeed<FurnitureComponent>
    {
        public FurnitureComponent[] Entities => new[]
        {
            new FurnitureComponent("COMP", 1,1,1) {Id = new Guid("663b8d4b-0968-4b6e-b54a-54ac6b9dfe42"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("7793fb49-f76c-49c4-ad17-85d8e7c92937"), BottomFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), TopFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), LeftFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), RightFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), FurnitureUnitId = new Guid("791c05fc-5008-4785-a734-b9225be8d3b3") },
            new FurnitureComponent("FRONT", 1,2,3) {Id = new Guid("94e9d857-e701-416c-95ab-5aad4793b6e2"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("7793fb49-f76c-49c4-ad17-85d8e7c92937"), BottomFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), TopFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), LeftFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), RightFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), FurnitureUnitId = new Guid("791c05fc-5008-4785-a734-b9225be8d3b3")  },

            new FurnitureComponent("Tür", 60, 180, 1) { Id = new Guid("d118b208-db6a-4c64-9708-de7e733fef07"), Type = FurnitureComponentTypeEnum.Corpus, BoardMaterialId = new Guid("b4346264-619f-4193-b2d0-7fab00b16b13"), BottomFoilId = new Guid("700b65e5-c5d5-4061-a9fc-d810e68ad006"), TopFoilId = new Guid("7a240dc3-8ab1-4f98-9eac-b9022f25b71b"), LeftFoilId = new Guid("700b65e5-c5d5-4061-a9fc-d810e68ad006"), RightFoilId = new Guid("7a240dc3-8ab1-4f98-9eac-b9022f25b71b"), ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), FurnitureUnitId = new Guid("ebf87e4c-50b8-402c-b694-ad29183e0bb1") },
            new FurnitureComponent("Tür", 42, 100, 1) { Id = new Guid("23f6c6a9-ca2d-4878-a3d7-9f26dba49882"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("b4346264-619f-4193-b2d0-7fab00b16b13"), BottomFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), TopFoilId = new Guid("700b65e5-c5d5-4061-a9fc-d810e68ad006"), LeftFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), RightFoilId = new Guid("700b65e5-c5d5-4061-a9fc-d810e68ad006"), ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), FurnitureUnitId = new Guid("ebf87e4c-50b8-402c-b694-ad29183e0bb1") },

            new FurnitureComponent("Regal", 22, 56, 1) { Id = new Guid("edaf2e97-4129-4a42-90d5-56cbaddf5f89"), Type = FurnitureComponentTypeEnum.Corpus, BoardMaterialId = new Guid("b4346264-619f-4193-b2d0-7fab00b16b13"), BottomFoilId = new Guid("7a240dc3-8ab1-4f98-9eac-b9022f25b71b"), TopFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), LeftFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), RightFoilId = new Guid("7a240dc3-8ab1-4f98-9eac-b9022f25b71b"), ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), FurnitureUnitId = new Guid("4105025c-e947-4d82-8e72-216582ec6b94") },
            new FurnitureComponent("Regal", 31, 44, 1) { Id = new Guid("f4058ceb-01ae-4eb6-a822-b4f1b0926734"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("b4346264-619f-4193-b2d0-7fab00b16b13"), BottomFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), TopFoilId = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"), LeftFoilId = new Guid("700b65e5-c5d5-4061-a9fc-d810e68ad006"), RightFoilId = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), FurnitureUnitId = new Guid("4105025c-e947-4d82-8e72-216582ec6b94") }
        };
        //public FurnitureComponent[] Entities => new FurnitureComponent[] { };
    }
}
