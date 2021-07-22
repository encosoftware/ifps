using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseDetailsDto
    {
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int Interval { get; set; }

        public PriceListDto Amount { get; set; }

        public GeneralExpenseDetailsDto(GeneralExpense generalExpense)
        {
            Name = generalExpense.GeneralExpenseRule.Name;
            FrequencyTypeId = generalExpense.GeneralExpenseRule.FrequencyTypeId;
            PaymentDate = generalExpense.PaymentDate;
            Interval = generalExpense.GeneralExpenseRule.Interval;
            Amount = new PriceListDto(generalExpense.Cost);
        }
    }
}
