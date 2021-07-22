using System;

namespace IFPS.Sales.Domain.Enums
{
    [Flags]
    public enum UserTypeFlag
    {
        User = 0,
        SalesPerson = 1,
        Customer = 2,
    }
}
