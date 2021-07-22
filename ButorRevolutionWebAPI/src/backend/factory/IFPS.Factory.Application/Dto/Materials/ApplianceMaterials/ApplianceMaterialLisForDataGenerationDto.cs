using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class ApplianceMaterialLisForDataGenerationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ApplianceMaterialLisForDataGenerationDto(ApplianceMaterial appliance)
        {
            Id = appliance.Id;
            Name = appliance.Code;
        }
    }
}
