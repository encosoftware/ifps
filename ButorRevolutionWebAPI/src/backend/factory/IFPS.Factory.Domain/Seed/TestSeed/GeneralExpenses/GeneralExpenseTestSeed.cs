using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class GeneralExpenseTestSeed : IEntitySeed<GeneralExpense>
    {
        public GeneralExpense[] Entities => new[]
        {
            new GeneralExpense(null, new DateTime(2019, 7, 30), 10001) { Id = 10000 }
        };
    }
}
