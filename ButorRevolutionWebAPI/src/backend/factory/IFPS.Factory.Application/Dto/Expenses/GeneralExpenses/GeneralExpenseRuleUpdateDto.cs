using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseRuleUpdateDto
    {
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public int Interval { get; set; }

        public PriceUpdateDto Amount { get; set; }

        public bool IsFixed { get; set; }

        public GeneralExpenseRule UpdateModelObject(GeneralExpenseRule generalExpenseRule)
        {
            generalExpenseRule.Name = Name;
            generalExpenseRule.Amount = Amount.CreateModelObject();
            generalExpenseRule.FrequencyTypeId = FrequencyTypeId;
            generalExpenseRule.StartDate = StartDate;
            generalExpenseRule.Interval = Interval;
            generalExpenseRule.ExpenseType = IsFixed ? GeneralExpenseRuleEnum.RecurrentFix : GeneralExpenseRuleEnum.RecurrentVariable;
            return generalExpenseRule;
        }
    }
}
