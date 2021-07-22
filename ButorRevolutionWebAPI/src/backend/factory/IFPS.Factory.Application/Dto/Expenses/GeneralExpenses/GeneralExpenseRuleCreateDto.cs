using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseRuleCreateDto
    {
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public int Interval { get; set; }

        public PriceCreateDto Amount { get; set; }

        public bool IsFixed { get; set; }


        public GeneralExpenseRule CreateModelObject()
        {
            return new GeneralExpenseRule(Name, StartDate, Interval,
                IsFixed ? GeneralExpenseRuleEnum.RecurrentFix : GeneralExpenseRuleEnum.RecurrentVariable,
                FrequencyTypeId, Amount.CreateModelObject());
        }
    }
}
