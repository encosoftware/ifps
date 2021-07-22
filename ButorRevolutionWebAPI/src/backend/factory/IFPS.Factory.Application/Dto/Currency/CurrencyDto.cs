using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CurrencyDto()
        {

        }
        public CurrencyDto(Currency currency) :this()
        {
            Id = currency.Id;
            Name = currency.Name;
        }
    }
}