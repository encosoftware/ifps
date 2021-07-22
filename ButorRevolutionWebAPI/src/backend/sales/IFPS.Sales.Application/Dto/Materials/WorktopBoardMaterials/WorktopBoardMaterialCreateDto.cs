using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class WorktopBoardMaterialCreateDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageCreateDto ImageCreateDto { get; set; }
        public DimensionCreateDto Dimension { get; set; }
        public bool HasFiberDirection { get; set; }
        public PriceCreateDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public WorktopBoardMaterial CreateModelObject()
        {
            return new WorktopBoardMaterial(Code, TransactionMultiplier)
            {
                Description = Description,
                CategoryId = CategoryId,
                Dimension = Dimension.CreateModelObject(),
                HasFiberDirection = HasFiberDirection,
            };
        }
    }
}
