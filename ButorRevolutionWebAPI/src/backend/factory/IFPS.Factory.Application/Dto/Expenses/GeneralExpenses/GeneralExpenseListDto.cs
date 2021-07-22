using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseListDto
    {
        public int Id { get; set; }
        public PriceListDto Amount { get; set; }
        public string Name { get; set; }
        public DateTime PaymentDate { get; set; }

        public GeneralExpenseListDto() { }

        public static Func<GeneralExpense, GeneralExpenseListDto> FromEntity = entity => new GeneralExpenseListDto
        {
            Id = entity.Id,
            Amount = new PriceListDto(entity.Cost),
            Name = entity.GeneralExpenseRule.Name,
            PaymentDate = entity.PaymentDate
        };
    }
}
