using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class GeneralExpenseRule : AggregateRoot
    {
        public string Name { get; set; }

        public FrequencyType FrequencyType { get; set; }
        public int FrequencyTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime LastPaymentDate { get; set; }

        public int Interval { get; set; }

        public Price Amount { get; set; }

        public GeneralExpenseRuleEnum ExpenseType { get; set; }

        public GeneralExpenseRule()
        {

        }

        public GeneralExpenseRule(string name, DateTime startDate, int interval, GeneralExpenseRuleEnum expenseType, int frequencyTypeId, Price amount)
        {
            Name = name;
            StartDate = startDate;
            Interval = interval;
            ExpenseType = expenseType;
            FrequencyTypeId = frequencyTypeId;
            Amount = amount;
        }
    }
}
