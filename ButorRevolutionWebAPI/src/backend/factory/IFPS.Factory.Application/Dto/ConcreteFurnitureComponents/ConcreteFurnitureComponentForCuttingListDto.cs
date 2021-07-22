using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class ConcreteFurnitureComponentForCuttingListDto
    {
        public int Id { get; set; }

        public int ConcreteFurnitureUnitId { get; set; }

        public Guid OrderGuid { get; set; }

        public string Name { get; set; }

        public double Width { get; set; }

        public double Length { get; set; }

        public Guid? BoardGuid { get; set; }

        public int FoilNumber { get; set; }

        public double CncDistance { get; set; }

        public int CncHoles { get; set; }

        public int CfcProductionStatus { get; set; }

        public ConcreteFurnitureComponentForCuttingListDto(ConcreteFurnitureComponent cfc)
        {
            Id = cfc.Id;
            ConcreteFurnitureUnitId = cfc.ConcreteFurnitureUnitId;
            Name = cfc.FurnitureComponent.Name;
            Width = cfc.FurnitureComponent.Width;
            Length = cfc.FurnitureComponent.Length;
            BoardGuid = cfc.FurnitureComponent.BoardMaterialId;
            OrderGuid = cfc.ConcreteFurnitureUnit.OrderId;
            if(cfc.FurnitureComponent.TopFoilId != null)
            {
                FoilNumber += 1;
            }
            if (cfc.FurnitureComponent.BottomFoilId != null)
            {
                FoilNumber += 1;
            }
            if (cfc.FurnitureComponent.RightFoilId != null)
            {
                FoilNumber += 1;
            }
            if (cfc.FurnitureComponent.LeftFoilId != null)
            {
                FoilNumber += 1;
            }
            CfcProductionStatus = (int)cfc.Status;
        }
    }
}
