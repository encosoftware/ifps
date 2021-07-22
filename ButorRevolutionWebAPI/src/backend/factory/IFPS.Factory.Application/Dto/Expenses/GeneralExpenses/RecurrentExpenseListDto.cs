using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class RecurrentExpenseListDto
    {
        public int Id { get; set; }
        public PriceListDto Amount { get; set; }
        public string Name { get; set; }

        public RecurrentExpenseListDto(GeneralExpenseRule generalExpenseRule)
        {
            Id = generalExpenseRule.Id;
            Amount = new PriceListDto(generalExpenseRule.Amount);
            Name = generalExpenseRule.Name;
        }
    }
}
