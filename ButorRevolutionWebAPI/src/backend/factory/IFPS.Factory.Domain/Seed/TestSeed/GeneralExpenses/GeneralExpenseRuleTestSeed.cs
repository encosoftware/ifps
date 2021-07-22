using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class GeneralExpenseRuleTestSeed : IEntitySeed<GeneralExpenseRule>
    {
        public GeneralExpenseRule[] Entities => new[]
        {
            new GeneralExpenseRule("Rule1", Clock.Now, 1, GeneralExpenseRuleEnum.RecurrentFix, 1, null) { Id = 10000 },
            new GeneralExpenseRule("Rule2",Clock.Now, 1,GeneralExpenseRuleEnum.RecurrentVariable, 2, null) { Id = 10001 },
            new GeneralExpenseRule("Rule3",Clock.Now, 1,GeneralExpenseRuleEnum.Single, 3, null) { Id = 10002 }
        };
    }
}
