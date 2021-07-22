using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CabinetMaterialDetailsDto
    {
        public double Height { get; set; }

        public double Depth { get; set; }

        public string Description { get; set; }

        public int OuterMaterialId { get; set; }

        public int InnerMaterialId { get; set; }

        public int BackMaterialId { get; set; }

        public int DoorMaterialId { get; set; }

        public CabinetMaterialDetailsDto(CabinetMaterial cabinet)
        {
            Height = cabinet.Height;
            Depth = cabinet.Depth;
            Description = cabinet.Description;
            OuterMaterialId = cabinet.OuterMaterialId;
            InnerMaterialId = cabinet.InnerMaterialId;
            BackMaterialId = cabinet.BackPanelMaterialId;
            DoorMaterialId = cabinet.DoorMaterialId;
        }
    }
}
