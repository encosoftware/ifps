using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseCreateDto
    {
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int Interval { get; set; }

        public PriceCreateDto Amount { get; set; }

        public GeneralExpense CreateModelObject()
        {
            var expenseRule = new GeneralExpenseRule(Name, PaymentDate, Interval,
                GeneralExpenseRuleEnum.Single, FrequencyTypeId, Amount.CreateModelObject());
            return new GeneralExpense(Amount.CreateModelObject(), PaymentDate, expenseRule.Id)
            {
                GeneralExpenseRule = expenseRule
            };
        }
    }
}
