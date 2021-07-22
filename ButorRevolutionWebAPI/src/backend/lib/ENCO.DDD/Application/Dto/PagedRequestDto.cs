namespace ENCO.DDD.Application.Dto
{
    public class PagedRequestDto
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 20;
    }
}
