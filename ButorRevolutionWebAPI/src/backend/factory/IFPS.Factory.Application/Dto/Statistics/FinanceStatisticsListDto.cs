namespace IFPS.Factory.Application.Dto
{
    public class FinanceStatisticsListDto
    {
        public int Month { get; set; }
        public double RecurringCost { get; set; }
        public double GeneralExpenseCost { get; set; }
        public double Income { get; set; }
        public string Currency { get; set; }
    }
}