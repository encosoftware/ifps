using System.Collections.Generic;

namespace ENCO.DDD.Application.Dto
{
    public class OrderedPagedRequestDto : PagedRequestDto
    {
        public List<OrderingDto> Orderings { get; set; } = new List<OrderingDto>();
    }
}
