using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class GeneralExpense : FullAuditedAggregateRoot
    {
        public Price Cost { get; set; }
        public DateTime PaymentDate { get; set; }
        public GeneralExpenseRule GeneralExpenseRule { get; set; }
        public int GeneralExpenseRuleId { get; set; }

        public GeneralExpense() { }

        public GeneralExpense(Price cost, DateTime paymentDate, int generalExpenseRuleId)
        {
            Cost = cost;
            PaymentDate = paymentDate;
            GeneralExpenseRuleId = generalExpenseRuleId;
        }
    }
}
