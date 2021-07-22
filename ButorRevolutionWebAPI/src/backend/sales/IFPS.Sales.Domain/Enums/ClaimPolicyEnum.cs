namespace IFPS.Sales.Domain.Enums
{
    public enum ClaimPolicyEnum
    {
        None = 0,

        #region Admin claims

        GetMaterials = 1,
        UpdateMaterials = 2,
        DeleteMaterials = 3,

        GetRoles = 4,
        UpdateRoles = 5,
        DeleteRoles = 6,

        GetVenues = 7,
        UpdateVenues = 8,
        DeleteVenues = 9,

        GetCompanies = 10,
        UpdateCompanies = 11,
        DeleteCompanies = 12,

        GetGroupingCategories = 13,
        UpdateGroupingCategories = 14,
        DeleteGroupingCategories = 15,

        GetFurnitureUnits = 16,
        UpdateFurnitureUnits = 17,
        DeleteFurnitureUnits = 18,

        GetUsers = 19,
        UpdateUsers = 20,
        DeleteUsers = 21,

        GetTestUsers = 100,
        UpdateTestUsers = 101,
        DeleteTestUsers = 102,
        GetTestFurnitureUnits = 103,
        UpdateTestFurnitureUnits = 104,
        DeleteTestFurnitureUnits = 105,
        #endregion

        #region Sales module claims
        GetAppointments = 200,
        UpdateAppointments = 201,
        DeleteAppointments = 202,

        GetOrders = 203,
        GetOrdersBySales = 204,
        UpdateOrders = 205,
        DeleteOrders = 206,

        GetOrderDocuments = 207,
        UpdateOrderDocuments = 208,        

        GetMessages = 209,
        UpdateMessages = 210,

        GetTickets = 211,
        GetTrends = 212,
        GetStatistics = 213,
        #endregion

        #region Customer module claims
        GetCustomerAppointments = 301,
        ApproveOrderDocuments = 302,
        GetOrdersByCustomer = 303,
        #endregion

        #region Partner module claims
        GetOrdersByPartner = 401,
        GetPartnerAppointments = 402,
        UpdateOrdersByPartner = 403,
        #endregion

        Other = 10000
    }
}
