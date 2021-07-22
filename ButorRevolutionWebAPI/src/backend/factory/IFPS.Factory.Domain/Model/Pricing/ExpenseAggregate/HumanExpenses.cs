using System;

namespace IFPS.Factory.Domain.Model
{
    public class HumanExpenses
    {
        public string Name { get; set; }

        public Price Cost { get; set; }

        public string ExpenseType { get; set; }

        public string Recurence { get; set; }

        public virtual Order Order { get; set; }
        public int? OrderId { get; set; }

        public User User { get; set; }
        public int? UserId { get; set; }

        public DateTime Date { get; set; }

        public HumanExpenses(string name, Price cost, string expenseType, string recurence, DateTime date)
        {
            Name = name;
            Cost = cost;
            ExpenseType = expenseType;
            Recurence = recurence;
            Date = date;
        }
    }
}
