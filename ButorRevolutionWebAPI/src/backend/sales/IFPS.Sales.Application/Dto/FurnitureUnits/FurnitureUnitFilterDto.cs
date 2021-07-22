using ENCO.DDD.Application.Dto;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitFilterDto : OrderedPagedRequestDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
