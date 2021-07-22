using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class RecurrentExpenseUpdateDto
    {
        public int Id { get; set; }
        public PriceUpdateDto Amount { get; set; }
        public string Name { get; set; }
        public RecurrentExpenseUpdateDto() { }

        public GeneralExpenseRule UpdateModelObject(GeneralExpenseRule generalExpenseRule)
        {
            generalExpenseRule.Name = Name;
            generalExpenseRule.Amount = Amount.CreateModelObject();
            return generalExpenseRule;
        }
    }
}
