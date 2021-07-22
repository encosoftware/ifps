using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CurrencyDto
    {        
        public int Id { get; set; }
        public string Name { get;  set; }

        public CurrencyDto()
        {

        }

        public CurrencyDto(Currency currency)
        {
            Id = currency.Id;
            Name = currency.Name;
        }
    }
}
