using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseRuleListDto
    {
        public int Id { get; set; }
        public PriceListDto Amount { get; set; }

        public string Name { get; set; }
        public int Interval { get; set; }

        public string Frequency { get; set; }

        public DateTime StartDate { get; set; }

        public GeneralExpenseRuleListDto() { }

        public static Func<GeneralExpenseRule, GeneralExpenseRuleListDto> FromEntity = entity => new GeneralExpenseRuleListDto
        {
            Id = entity.Id,
            Amount = new PriceListDto(entity.Amount),
            Name = entity.Name,
            Interval = entity.Interval,
            Frequency = entity.FrequencyType.CurrentTranslation.Name,
            StartDate = entity.StartDate
        };
    }
}
