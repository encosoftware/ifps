using IFPS.Factory.Application.Extensions;
using IFPS.Factory.Domain.Dbos;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class RequiredMaterialsListDto
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public List<SupplierDropdownListDto> Suppliers { get; set; }
        public DateTime Deadline { get; set; }

        public RequiredMaterialsListDto(){ }

        public static Func<RequiredMaterial, RequiredMaterialsListDto> FromEntity = entity => new RequiredMaterialsListDto()
        {
            Id = entity.Id,
            OrderName = entity.Order.OrderName,
            WorkingNumber = entity.Order.WorkingNumber,
            MaterialCode = entity.Material.Code,
            Name = entity.Material.Description,
            Amount = entity.RequiredAmount,
            Suppliers = entity.Material.Packages.Select(ent => new SupplierDropdownListDto(ent.SupplierId, ent.Supplier.Name)).Distinct(new SupplierDtoEqualityComparer()).ToList(),
            Deadline = entity.Order.Deadline
        };
    }
}
