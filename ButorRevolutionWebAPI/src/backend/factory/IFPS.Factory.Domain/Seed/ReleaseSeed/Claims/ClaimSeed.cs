using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class ClaimSeed : IEntitySeed<Claim>
    {
        public Claim[] Entities => new[]
        {
            //Admin
            new Claim(ClaimPolicyEnum.GetRoles, 1) { Id = 1 },
            new Claim(ClaimPolicyEnum.UpdateRoles, 1) { Id = 2 },
            new Claim(ClaimPolicyEnum.DeleteRoles, 1) { Id = 3 },

            new Claim(ClaimPolicyEnum.GetCompanies, 1) { Id = 4 },
            new Claim(ClaimPolicyEnum.UpdateCompanies, 1) { Id = 5 },
            new Claim(ClaimPolicyEnum.DeleteCompanies, 1) { Id = 6 },

            new Claim(ClaimPolicyEnum.GetUsers, 1) { Id = 7 },
            new Claim(ClaimPolicyEnum.UpdateUsers, 1) { Id = 8 },
            new Claim(ClaimPolicyEnum.DeleteUsers, 1) { Id = 9 },

            new Claim(ClaimPolicyEnum.GetMaterialPackages, 1) { Id = 57 },
            new Claim(ClaimPolicyEnum.UpdateMaterialPackages, 1) { Id = 58 },
            new Claim(ClaimPolicyEnum.DeleteMaterialPackages, 1) { Id = 59 },

            //Production
            new Claim(ClaimPolicyEnum.GetAssemblies, 2) { Id = 10 },
            new Claim(ClaimPolicyEnum.GetCncs, 2) { Id = 11 },
            new Claim(ClaimPolicyEnum.GetCuttings, 2) { Id = 12 },
            new Claim(ClaimPolicyEnum.GetEdgeBandings, 2) { Id = 13 },
            new Claim(ClaimPolicyEnum.GetSortings, 2) { Id = 69 },
            new Claim(ClaimPolicyEnum.GetPackings, 2) { Id = 70 },

            new Claim(ClaimPolicyEnum.UpdateProductionProcess, 2) { Id = 71 },

            new Claim(ClaimPolicyEnum.GetOrderSchedulings, 2) { Id = 14 },
            new Claim(ClaimPolicyEnum.UpdateOrderSchedulings, 2) { Id = 15 },

            new Claim(ClaimPolicyEnum.GetOptimizations, 2) { Id = 60 },
            new Claim(ClaimPolicyEnum.UpdateOptimizations, 2) { Id = 61 },

            new Claim(ClaimPolicyEnum.GetWorkstations, 2) { Id = 16 },
            new Claim(ClaimPolicyEnum.UpdateWorkstations, 2) { Id = 17 },
            new Claim(ClaimPolicyEnum.DeleteWorkstations, 2) { Id = 18 },

            new Claim(ClaimPolicyEnum.GetCameras, 2) { Id = 19 },
            new Claim(ClaimPolicyEnum.UpdateCameras, 2) { Id = 20 },
            new Claim(ClaimPolicyEnum.DeleteCameras, 2) { Id = 21 },

            new Claim(ClaimPolicyEnum.GetMachines, 2) { Id = 22 },
            new Claim(ClaimPolicyEnum.UpdateMachines, 2) { Id = 23 },
            new Claim(ClaimPolicyEnum.DeleteMachines, 2) { Id = 24 },

            new Claim(ClaimPolicyEnum.GetWorkloads, 2) { Id = 65 },
            new Claim(ClaimPolicyEnum.GetFurnitureUnits, 2) { Id = 72 },            

            //Financial
            new Claim(ClaimPolicyEnum.GetGeneralExpenses, 3) { Id = 25 },
            new Claim(ClaimPolicyEnum.UpdateGeneralExpenses, 3) { Id = 26 },
            new Claim(ClaimPolicyEnum.DeleteGeneralExpenses, 3) { Id = 27 },

            new Claim(ClaimPolicyEnum.GetOrderExpenses, 3) { Id = 66 },

            new Claim(ClaimPolicyEnum.GetFinanceStatistics, 3) { Id = 67 },

            //Supply
            new Claim(ClaimPolicyEnum.GetCargos, 4) { Id = 28 },
            new Claim(ClaimPolicyEnum.UpdateCargos, 4) { Id = 29 },
            new Claim(ClaimPolicyEnum.DeleteCargos, 4) { Id = 30 },

            new Claim(ClaimPolicyEnum.GetRequiredMaterials, 4) { Id = 31 },
            new Claim(ClaimPolicyEnum.UpdateRequiredMaterials, 4) { Id = 32 },

            //Warehouse
            new Claim(ClaimPolicyEnum.GetInspections, 5) { Id = 33 },
            new Claim(ClaimPolicyEnum.UpdateInspections, 5) { Id = 34 },
            new Claim(ClaimPolicyEnum.DeleteInspections, 5) { Id = 35 },

            new Claim(ClaimPolicyEnum.GetStocks, 5) { Id = 36 },
            new Claim(ClaimPolicyEnum.UpdateStocks, 5) { Id = 37 },
            new Claim(ClaimPolicyEnum.DeleteStocks, 5) {Id = 38 },

            new Claim(ClaimPolicyEnum.GetStorageCells, 5) { Id = 39 },
            new Claim(ClaimPolicyEnum.UpdateStorageCells, 5) { Id = 40 },
            new Claim(ClaimPolicyEnum.DeleteStorageCells, 5) { Id = 41 },

            new Claim(ClaimPolicyEnum.GetStorages, 5) { Id = 42 },
            new Claim(ClaimPolicyEnum.UpdateStorages, 5) { Id = 43 },
            new Claim(ClaimPolicyEnum.DeleteStorages, 5) { Id = 44 },

            new Claim(ClaimPolicyEnum.GetStockStatistics, 5) { Id = 68 }
        };
    }
}