using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class ConcreteFurnitureComponentInformationListDto
    {
        public ImageListDto Image { get; set; }
        public string WorkingNumber { get; set; }
        public string FurnitureComponentCode { get; set; }
        public string TopFoilCode { get; set; }
        public string BottomFoilCode { get; set; }
        public string LeftFoilCode { get; set; }
        public string RightFoilCode { get; set; }

        public static Expression<Func<ConcreteFurnitureComponent, ConcreteFurnitureComponentInformationListDto>> Projection
        {
            get
            {
                return entity => new ConcreteFurnitureComponentInformationListDto
                {
                    Image = new ImageListDto(entity.QRCode),
                    WorkingNumber = entity.ConcreteFurnitureUnit.Order.WorkingNumber,
                    FurnitureComponentCode = entity.ConcreteFurnitureUnit.FurnitureUnit.Code,
                    TopFoilCode = entity.FurnitureComponent.TopFoil.Code,
                    BottomFoilCode = entity.FurnitureComponent.BottomFoil.Code,
                    LeftFoilCode = entity.FurnitureComponent.LeftFoil.Code,
                    RightFoilCode = entity.FurnitureComponent.RightFoil.Code,
                };
            }
        }
    }
}
