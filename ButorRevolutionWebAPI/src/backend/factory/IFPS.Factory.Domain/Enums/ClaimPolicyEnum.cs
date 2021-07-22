namespace IFPS.Factory.Domain.Enums
{
    public enum ClaimPolicyEnum
    {
        None = 0,

        //Admin
        GetRoles = 1,
        UpdateRoles = 2,
        DeleteRoles = 3,

        GetCompanies = 4,
        UpdateCompanies = 5,
        DeleteCompanies = 6,

        GetUsers = 7,
        UpdateUsers = 8,
        DeleteUsers = 9,

        GetMaterialPackages = 10,
        UpdateMaterialPackages = 11,
        DeleteMaterialPackages = 12,

        //Production
        GetOrderSchedulings = 13,
        UpdateOrderSchedulings = 14,

        GetOptimizations = 15,
        UpdateOptimizations = 53,

        GetCuttings = 16,
        GetCncs = 17,
        GetEdgeBandings = 18,
        GetAssemblies = 19,
        GetPackings = 200,
        GetSortings = 201,

        UpdateProductionProcess = 202,

        GetCameras = 20,
        UpdateCameras = 21,
        DeleteCameras = 22,

        GetWorkstations = 23,
        UpdateWorkstations = 24,
        DeleteWorkstations = 25,

        GetMachines = 26,
        UpdateMachines = 27,
        DeleteMachines = 28,

        GetWorkloads = 29,
        GetFurnitureUnits = 54,

        //Supply
        GetCargos = 30,
        UpdateCargos = 31,
        DeleteCargos = 32,

        GetRequiredMaterials = 33,
        UpdateRequiredMaterials = 34,

        //Financial
        GetGeneralExpenses = 35,
        UpdateGeneralExpenses = 36,
        DeleteGeneralExpenses = 37,

        GetOrderExpenses = 38,

        GetFinanceStatistics = 39,

        //Warehouse
        GetInspections = 40,
        UpdateInspections = 41,
        DeleteInspections = 42,

        GetStocks = 43,
        UpdateStocks = 44,
        DeleteStocks = 45,

        GetStorageCells = 46,
        UpdateStorageCells = 47,
        DeleteStorageCells = 48,

        GetStorages = 49,
        UpdateStorages = 50,
        DeleteStorages = 51,

        GetStockStatistics = 52,

        //Test
        GetTestUsers = 100,
        UpdateTestUsers = 101,
        DeleteTestUsers = 102,
        GetTestFurnitureUnits = 103,
        UpdateTestFurnitureUnits = 104,
        DeleteTestFurnitureUnits = 105,

        Other = 1000
    }
}
