using IFPS.Factory.Application.Dto;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Extensions
{
    public class SupplierDtoEqualityComparer : IEqualityComparer<SupplierDropdownListDto>
    {
        public bool Equals(SupplierDropdownListDto existingSupplier, SupplierDropdownListDto newSupplier)
        {
            return existingSupplier.Id == newSupplier.Id;
        }

        public int GetHashCode(SupplierDropdownListDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
