using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceMaterialsListForDropdownDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ApplianceMaterialsListForDropdownDto(ApplianceMaterial applianceMaterial)
        {
            Id = applianceMaterial.Id;
            Name = applianceMaterial.Description;
        }
    }
}
