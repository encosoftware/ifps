using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderSeed : IEntitySeed<Order>
    {
        public Order[] Entities => new[]
        {
            new Order("FACTORY_Order 001 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("D116F5CA-58AF-47A4-92A2-1FB2ABC31F51"), CurrentTicketId = 14, WorkingNumber = "MZ/X-42", CompanyId = 1, FirstPaymentId = 1, SecondPaymentId = 15, OptimizationId = Guid.Parse("b65394b2-77d8-4936-bbd4-dda58adf0d71") },
            new Order("FACTORY_Order 002 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("409F08EE-7FAD-43D2-A6CB-E0ED274D9CB9"), CurrentTicketId = 2, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 2, SecondPaymentId = 16, OptimizationId = Guid.Parse("b65394b2-77d8-4936-bbd4-dda58adf0d71") },

            new Order("FACTORY_Order 003 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("627966DE-6BD4-4EED-B07C-8D78046509C8"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 6, SecondPaymentId = 20 },
            new Order("FACTORY_Order 004 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("B4B68154-674F-44E0-86C5-8FF47914DC1C"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 7, SecondPaymentId = 21 },
            new Order("FACTORY_Order 005 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("3B290920-BDA8-41DA-BF7E-B3F8770CD5B5"), CurrentTicketId = 8, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 8, SecondPaymentId = 22 },
            new Order("FACTORY_Order 006 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("AB6F5475-C916-40D9-989A-59ECDA3833A9"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 9, SecondPaymentId = 23 },
            new Order("FACTORY_Order 007 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("E090C246-CF62-4BDC-B545-E1B50C4BF8B1"), CurrentTicketId = 10, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 10, SecondPaymentId = 24 },
            new Order("FACTORY_Order 008 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("FBABFC83-F103-4E1C-9592-365E62CAF909"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 11, SecondPaymentId = 25 },
            new Order("FACTORY_Order 009 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("38281E86-36AC-4AF7-A0C5-B04FAE72499B"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 12, SecondPaymentId = 26 },
            new Order("FACTORY_Order 010 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("BE290328-EDF9-4A23-8225-8058F107AD98"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, FirstPaymentId = 13, SecondPaymentId = 27 },

            new Order("FACTORY_Order 011 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("0C60CDBC-FCE3-4833-8FFA-D46664A68DA3"), CurrentTicketId = 7, WorkingNumber = "MZ/X-567", CompanyId = 1, FirstPaymentId = 14, SecondPaymentId = 28, },

            // Order Scheduling
            new Order("FACTORY_Order 012 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("0D9F83DC-9143-49E5-9DD7-8DC702F130CB"), CurrentTicketId = 14, WorkingNumber = "MZ/X-503", CompanyId = 1, Completion = 0, EstimatedProcessTime = 60000, FirstPaymentId = 3, SecondPaymentId = 17, OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") },
            new Order("FACTORY_Order 013 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("83B6E1CB-215B-42D2-93BB-5DBA12FE039E"), CurrentTicketId = 14, WorkingNumber = "MZ/X-880", CompanyId = 1, Completion = 42, EstimatedProcessTime = 42000, FirstPaymentId = 4, SecondPaymentId = 18, OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") },
            new Order("FACTORY_Order 014 (seed)", deadline: Clock.Now.AddMonths(2)) { Id = new Guid("50EBFCBD-33DC-4BF4-B82D-6336E1EA6F48"), CurrentTicketId = 14, WorkingNumber = "MZ/X-007", CompanyId = 1, Completion = 100, EstimatedProcessTime = 120000, FirstPaymentId = 5, SecondPaymentId = 19 },
        };

        //public Order[] Entities => new Order[] { };
    }
}
