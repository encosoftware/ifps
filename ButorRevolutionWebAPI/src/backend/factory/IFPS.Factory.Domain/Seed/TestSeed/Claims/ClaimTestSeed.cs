using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class ClaimTestSeed : IEntitySeed<Claim>
    {
        public Claim[] Entities => new[]
        {
            new Claim(ClaimPolicyEnum.GetRoles, 10000) {Id = 10000 },
            new Claim(ClaimPolicyEnum.UpdateRoles, 10000) {Id = 10001 },
            new Claim(ClaimPolicyEnum.DeleteRoles, 10000) {Id = 10002 },

            new Claim(ClaimPolicyEnum.GetCompanies, 10000) {Id = 10003 },
            new Claim(ClaimPolicyEnum.UpdateCompanies, 10000) {Id = 10004 },
            new Claim(ClaimPolicyEnum.DeleteCompanies, 10000) {Id = 10005 },

            new Claim(ClaimPolicyEnum.GetUsers, 10000) {Id = 10006 },
            new Claim(ClaimPolicyEnum.UpdateUsers, 10000) {Id = 10007 },
            new Claim(ClaimPolicyEnum.DeleteUsers, 10000) {Id = 10008 },

            new Claim(ClaimPolicyEnum.GetMaterialPackages, 10000) { Id = 10057 },
            new Claim(ClaimPolicyEnum.UpdateMaterialPackages, 10000) { Id = 10058 },
            new Claim(ClaimPolicyEnum.DeleteMaterialPackages, 10000) { Id = 10059 },

            new Claim(ClaimPolicyEnum.GetAssemblies, 10000) {Id = 10009 },
            new Claim(ClaimPolicyEnum.GetCncs, 10000) {Id = 10010 },
            new Claim(ClaimPolicyEnum.GetCuttings, 10000) {Id = 10011 },
            new Claim(ClaimPolicyEnum.GetEdgeBandings, 10000) {Id = 10012 },

            new Claim(ClaimPolicyEnum.GetOrderSchedulings, 10000) {Id = 10013 },
            new Claim(ClaimPolicyEnum.UpdateOrderSchedulings, 10000) {Id = 10113 },

            new Claim(ClaimPolicyEnum.GetOptimizations, 10000) { Id = 10060 },
            new Claim(ClaimPolicyEnum.UpdateOptimizations, 10000) { Id = 10061 },

            new Claim(ClaimPolicyEnum.GetWorkstations, 10000) {Id = 10116 },
            new Claim(ClaimPolicyEnum.UpdateWorkstations, 10000) {Id = 10117 },
            new Claim(ClaimPolicyEnum.DeleteWorkstations, 10000) {Id = 10118 },

            new Claim(ClaimPolicyEnum.GetCameras, 10000) {Id = 10119 },
            new Claim(ClaimPolicyEnum.UpdateCameras, 10000) {Id = 10120 },
            new Claim(ClaimPolicyEnum.DeleteCameras, 10000) {Id = 10121 },

            new Claim(ClaimPolicyEnum.GetMachines, 10000) {Id = 10122 },
            new Claim(ClaimPolicyEnum.UpdateMachines, 10000) {Id = 10123 },
            new Claim(ClaimPolicyEnum.DeleteMachines, 10000) {Id = 10124 },

            new Claim(ClaimPolicyEnum.GetWorkloads, 10000) { Id = 10065 },

            new Claim(ClaimPolicyEnum.GetCargos, 10000) {Id = 10014 },
            new Claim(ClaimPolicyEnum.UpdateCargos, 10000) {Id = 10015 },
            new Claim(ClaimPolicyEnum.DeleteCargos, 10000) {Id = 10016 },

            new Claim(ClaimPolicyEnum.GetGeneralExpenses, 10000) {Id = 10017 },
            new Claim(ClaimPolicyEnum.UpdateGeneralExpenses, 10000) {Id = 10018 },
            new Claim(ClaimPolicyEnum.DeleteGeneralExpenses, 10000) {Id = 10019 },

            new Claim(ClaimPolicyEnum.GetOrderExpenses, 10000) { Id = 10066 },
            new Claim(ClaimPolicyEnum.GetFinanceStatistics, 10000) { Id = 10067 },

            new Claim(ClaimPolicyEnum.GetInspections, 10000) {Id = 10020 },
            new Claim(ClaimPolicyEnum.UpdateInspections, 10000) {Id = 10021 },
            new Claim(ClaimPolicyEnum.DeleteInspections, 10000) {Id = 10022 },

            new Claim(ClaimPolicyEnum.GetStocks, 10000) {Id = 10023 },
            new Claim(ClaimPolicyEnum.UpdateStocks, 10000) {Id = 10024 },
            new Claim(ClaimPolicyEnum.DeleteStocks, 10000) {Id = 10025 },

            new Claim(ClaimPolicyEnum.GetStorageCells, 10000) {Id = 10026 },
            new Claim(ClaimPolicyEnum.UpdateStorageCells, 10000) {Id = 10027 },
            new Claim(ClaimPolicyEnum.DeleteStorageCells, 10000) {Id = 10028 },

            new Claim(ClaimPolicyEnum.GetStorages, 10000) {Id = 10029 },
            new Claim(ClaimPolicyEnum.UpdateStorages, 10000) {Id = 10030 },
            new Claim(ClaimPolicyEnum.DeleteStorages, 10000) {Id = 10031 },

            new Claim(ClaimPolicyEnum.GetRequiredMaterials, 10000) {Id = 10032 },
            new Claim(ClaimPolicyEnum.UpdateRequiredMaterials, 10000) {Id = 10033 },
            new Claim(ClaimPolicyEnum.GetFurnitureUnits, 10001) {Id = 10034 },

            new Claim(ClaimPolicyEnum.GetStockStatistics, 10000) { Id = 10068 }
        };
    }
}