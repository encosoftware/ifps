namespace IFPS.Factory.Application.Dto
{
    public class GetOldestYearDto
    {
        public int OldestYear { get; set; }

        public GetOldestYearDto(int oldestYear)
        {
            OldestYear = oldestYear;
        }
    }
}
