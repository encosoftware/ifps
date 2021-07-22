using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class OrderSeed : IEntitySeed<Order>
    {
        public Order[] Entities => new[]
        {
            new Order("SALES_Order 001 (seed)", 2, 2, new DateTime(2020, 12, 13), null) { Id = new Guid("5C75E657-4BB7-4791-A829-5E85C2EA7D12"), CurrentTicketId = 1, FirstPaymentId = 1, SecondPaymentId = 2, WorkingNumberYear = 2019, WorkingNumberSerial = 42, ContractDate = Clock.Now, OfferInformationId = 1 },
            new Order("SALES_Order 002 (seed)", 2, 4, new DateTime(2019, 12, 31), null) { Id = new Guid("2418B030-A64B-4724-9702-964CF5EB04C6"), CurrentTicketId = 3, FirstPaymentId = 1, SecondPaymentId = 2, WorkingNumberYear = 2020, WorkingNumberSerial = 45, ContractDate = Clock.Now },
            new Order("SALES_Order 003 (seed)", 2, 3, new DateTime(2020, 02, 15), null) { Id = new Guid("DD504088-3F36-4AD9-9CC1-84545AFA497D"), CurrentTicketId = 2, FirstPaymentId = 1, SecondPaymentId = 2, WorkingNumberYear = 2021, WorkingNumberSerial = 700, ContractDate = Clock.Now, Aggrement = "This template is a business contract between a product development or design agency and a company who wishes to contract them for design services." },
            new Order("SALES_Order 004 (seed)", 2, 3, new DateTime(2019, 12, 31), null) { Id = new Guid("C7237D77-ADCF-45CB-8835-4DB99DF213FA"), CurrentTicketId = 1, FirstPaymentId = 3, SecondPaymentId = 4, WorkingNumberYear = 2022, WorkingNumberSerial = 801, ContractDate = Clock.Now },
            new Order("SALES_Order 005 (seed)", 2, 4, new DateTime(2020, 04, 20), null) { Id = new Guid("791FF638-7B97-4118-AE40-677EDAD1A64D"), CurrentTicketId = 2, FirstPaymentId = 5, SecondPaymentId = 6, WorkingNumberYear = 2023, WorkingNumberSerial = 802, ContractDate = Clock.Now },
            new Order("SALES_Order 006 (seed)", 2, 5, new DateTime(2019, 05, 12), null) { Id = new Guid("509B7D7A-46B5-42A4-A944-4C46BC8B91FB"), CurrentTicketId = 1, FirstPaymentId = 7, SecondPaymentId = 8, WorkingNumberYear = 2024, WorkingNumberSerial = 803, ContractDate = Clock.Now, Aggrement = "This template is a business contract between a product development or design agency and a company who wishes to contract them for design services." },
            new Order("SALES_Order 007 (seed)", 2, 6, new DateTime(2019, 08, 31), null) { Id = new Guid("55E7F6D7-9444-400A-BFB4-987DC59C8A55"), CurrentTicketId = 4, FirstPaymentId = 9, SecondPaymentId = 10, WorkingNumberYear = 2025, WorkingNumberSerial = 804, ContractDate = Clock.Now },
            new Order("SALES_Order 008 (seed)", 2, 7, new DateTime(2020, 02, 13), null) { Id = new Guid("4C9602FD-6139-4D35-9D7C-5468AC4F2F64"), CurrentTicketId = 4, FirstPaymentId = 11, SecondPaymentId = 12, WorkingNumberYear = 2026, WorkingNumberSerial = 805, ContractDate = Clock.Now, Aggrement = "This template is a business contract between a product development or design agency and a company who wishes to contract them for design services." },
            new Order("SALES_Order 009 (seed)", 2, 8, new DateTime(2020, 03, 10), null) { Id = new Guid("3E2D98BE-1C68-4DB6-B180-1C6D6CD03ACC"), CurrentTicketId = 3, FirstPaymentId = 13, SecondPaymentId = 14, WorkingNumberYear = 2027, WorkingNumberSerial = 806, ContractDate = Clock.Now },
            new Order("SALES_Order 010 (seed)", 2, 9, new DateTime(2020, 07, 01), null) { Id = new Guid("3C9361FB-23C2-4952-8A27-F19CE4D2603D"), CurrentTicketId = 1, FirstPaymentId = 15, SecondPaymentId = 16, WorkingNumberYear = 2028, WorkingNumberSerial = 807, ContractDate = Clock.Now, Aggrement = "This template is a business contract between a product development or design agency and a company who wishes to contract them for design services." },
            new Order("SALES_Order 011 (seed)", 2, 10, new DateTime(2020, 10, 20), null) { Id = new Guid("73D46114-FE92-4AB8-B8A6-719B8206A6BC"), CurrentTicketId = 3, FirstPaymentId = 17, SecondPaymentId = 18, WorkingNumberYear = 2029, WorkingNumberSerial = 808, ContractDate = Clock.Now },
        };
        //public Order[] Entities => new Order[] { };
    }
}