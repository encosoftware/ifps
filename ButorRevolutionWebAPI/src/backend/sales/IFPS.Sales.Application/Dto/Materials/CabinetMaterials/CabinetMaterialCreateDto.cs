using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class CabinetMaterialCreateDto
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public int OuterMaterialId { get; set; }
        public int InnerMaterialId { get; set; }
        public int BackPanelMaterialId { get; set; }
        public int DoorMaterialId { get; set; }
        public string Description { get; set; }

        public CabinetMaterial CreateModelObject(Guid orderId)
        {
            return new CabinetMaterial()
            {
                BackPanelMaterialId = BackPanelMaterialId,
                DoorMaterialId = DoorMaterialId,
                InnerMaterialId = InnerMaterialId,
                OuterMaterialId = OuterMaterialId,
                Height = Height,
                Depth = Width,
                Description = Description,
                OrderId = orderId
            };
        }

    }
}
