namespace IFPS.Factory.Application.Dto
{
    public class StockStatisticsQuantityDto
    {
        public int WeekNumber { get; set; }
        public int Quantity { get; set; }

        public StockStatisticsQuantityDto(int weekNumber, int quantity)
        {
            WeekNumber = weekNumber;
            Quantity = quantity;
        }
    }
}