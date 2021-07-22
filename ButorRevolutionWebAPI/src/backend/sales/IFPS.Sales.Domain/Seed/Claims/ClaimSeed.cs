using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class ClaimSeed : IEntitySeed<Claim>
    {
        public Claim[] Entities => new[]
        {
            new Claim(ClaimPolicyEnum.GetMaterials, 1) { Id = 1, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateMaterials, 1) { Id = 2, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteMaterials, 1) { Id = 3, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetRoles, 1) { Id = 4, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateRoles, 1) { Id = 5, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteRoles, 1) { Id = 6, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetVenues, 1) { Id = 7, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateVenues, 1) { Id = 8, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteVenues, 1) { Id = 9, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetCompanies, 1) { Id = 10, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateCompanies, 1) { Id = 11, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteCompanies, 1) { Id = 12, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetGroupingCategories, 1) { Id = 13, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateGroupingCategories, 1) { Id = 14, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteGroupingCategories, 1) { Id = 15, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetFurnitureUnits, 1) { Id = 16, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateFurnitureUnits, 1) { Id = 17, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteFurnitureUnits, 1) { Id = 18, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetUsers, 1) { Id = 19, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateUsers, 1) { Id = 20, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteUsers, 1) { Id = 21, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetAppointments, 2) { Id = 22, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateAppointments, 2) { Id = 23, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteAppointments, 2) { Id = 24, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetOrders, 2) { Id = 25, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateOrders, 2) { Id = 26, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.DeleteOrders, 2) { Id = 27, ClaimType = ClaimTypeEnum.Delete },

            new Claim(ClaimPolicyEnum.GetOrderDocuments, 2) { Id = 28, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateOrderDocuments, 2) { Id = 29, ClaimType = ClaimTypeEnum.Update },

            new Claim(ClaimPolicyEnum.GetMessages, 2) { Id = 30, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateMessages, 2) { Id = 31, ClaimType = ClaimTypeEnum.Update },

            new Claim(ClaimPolicyEnum.GetOrdersBySales, 2) { Id = 32, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.GetTickets, 2) { Id = 33, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.GetTrends, 2) { Id = 34, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.GetStatistics, 2) { Id = 35, ClaimType = ClaimTypeEnum.Get },

            new Claim(ClaimPolicyEnum.GetOrdersByPartner, 3) { Id = 36, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.GetPartnerAppointments, 3) { Id = 37, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.UpdateOrdersByPartner, 3) { Id = 38, ClaimType = ClaimTypeEnum.Update },

            new Claim(ClaimPolicyEnum.GetCustomerAppointments, 4) { Id = 39, ClaimType = ClaimTypeEnum.Get },
            new Claim(ClaimPolicyEnum.ApproveOrderDocuments, 4) { Id = 40, ClaimType = ClaimTypeEnum.Update },
            new Claim(ClaimPolicyEnum.GetOrdersByCustomer, 4) { Id = 41, ClaimType = ClaimTypeEnum.Get }
        };
    }
}