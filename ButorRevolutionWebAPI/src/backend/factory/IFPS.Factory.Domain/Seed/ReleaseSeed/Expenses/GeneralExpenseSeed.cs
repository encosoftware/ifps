using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class GeneralExpenseSeed : IEntitySeed<GeneralExpense>
    {
        public GeneralExpense[] Entities => new[]
        {
            new GeneralExpense(null, new DateTime(2019, 7, 30), 2) { Id = 1 }
        };
        
        //public GeneralExpense[] Entities => new GeneralExpense[] { };
    }
}
