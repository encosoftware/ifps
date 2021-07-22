using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseRuleDetailsDto
    {
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public int Interval { get; set; }

        public PriceListDto Amount { get; set; }

        public bool IsFixed { get; set; }

        public GeneralExpenseRuleDetailsDto(GeneralExpenseRule generalExpenseRule)
        {
            Name = generalExpenseRule.Name;
            FrequencyTypeId = generalExpenseRule.FrequencyTypeId;
            StartDate = generalExpenseRule.StartDate;
            Interval = generalExpenseRule.Interval;
            Amount = new PriceListDto(generalExpenseRule.Amount);
            IsFixed = generalExpenseRule.ExpenseType == GeneralExpenseRuleEnum.RecurrentFix ? true : false;
        }
    }
}
