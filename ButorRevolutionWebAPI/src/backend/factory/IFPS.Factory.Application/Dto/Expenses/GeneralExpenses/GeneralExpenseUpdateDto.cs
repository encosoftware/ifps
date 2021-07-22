using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseUpdateDto
    {
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int Interval { get; set; }

        public PriceUpdateDto Cost { get; set; }

        public GeneralExpense UpdateModelObject(GeneralExpense generalExpense)
        {
            generalExpense.GeneralExpenseRule.Name = Name;
            generalExpense.Cost = Cost.CreateModelObject();
            generalExpense.GeneralExpenseRule.FrequencyTypeId = FrequencyTypeId;
            generalExpense.PaymentDate = PaymentDate;
            generalExpense.GeneralExpenseRule.Interval = Interval;
            return generalExpense;
        }
    }
}
