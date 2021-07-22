using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class WebshopOrderSeed : IEntitySeed<WebshopOrder>
    {
        public WebshopOrder[] Entities => new[]
        {
            new WebshopOrder("BR-984763", 1, null) { Id = new Guid("23cf9320-3062-4db4-aa29-a0594a726f3f"), WorkingNumberSerial = 457, WorkingNumberYear = 2843, BasketId = 1 },
            new WebshopOrder("BR-100274", 1, null) { Id = new Guid("1f8087e2-8134-4801-af76-8276fbf246b7"), WorkingNumberSerial = 449, WorkingNumberYear = 2844, BasketId = 1 },
            new WebshopOrder("BR-605020", 1, null) { Id = new Guid("989d1357-1aed-4ee9-9b40-65e494a23784"), WorkingNumberSerial = 423, WorkingNumberYear = 2845, BasketId = 1 },
            new WebshopOrder("BR-001001", 1, null) { Id = new Guid("6eaa291b-f125-4f0a-a0b3-5c878d251ba3"), WorkingNumberSerial = 425, WorkingNumberYear = 2019, BasketId = 1 },
            new WebshopOrder("BR-002002", 1, null) { Id = new Guid("98e32021-bcaa-497f-bbf9-e61853ecf295"), WorkingNumberSerial = 410, WorkingNumberYear = 2020, BasketId = 1 },
            new WebshopOrder("BR-003003", 1, null) { Id = new Guid("20b33f75-09b7-4bdc-9470-e49db63dc3de"), WorkingNumberSerial = 467, WorkingNumberYear = 2020, BasketId = 1 },
            new WebshopOrder("BR-700402", 1, null) { Id = new Guid("1055b58c-c922-4f13-a35c-af0e56083a51"), WorkingNumberSerial = 489, WorkingNumberYear = 2020, BasketId = 1 },
            new WebshopOrder("BR-080350", 1, null) { Id = new Guid("fd53912a-9a3c-4586-86b1-d704b5fbb180"), WorkingNumberSerial = 477, WorkingNumberYear = 2020, BasketId = 1 },
            new WebshopOrder("BR-930107", 1, null) { Id = new Guid("52f9338b-263c-4054-81dd-70d6345a171e"), WorkingNumberSerial = 454, WorkingNumberYear = 2020, BasketId = 1 },
            new WebshopOrder("BR-031008", 1, null) { Id = new Guid("c654a3f5-3949-4df2-bf6d-ee4a62622368"), WorkingNumberSerial = 499, WorkingNumberYear = 2020, BasketId = 1 },
        };
        //public WebshopOrder[] Entities => new WebshopOrder[] { };
    }
}
