using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class CurrencyAppService : ApplicationService, ICurrencyAppService
    {
        private readonly ICurrencyRepository currencyRepository;
        public CurrencyAppService(IApplicationServiceDependencyAggregate aggregate,
            ICurrencyRepository currencyRepository) : base(aggregate)
        {
            this.currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyDto>> GetCurrenciesListAsync()
        {
            var result = await currencyRepository.GetAllListAsync();
            return result.Select(x => new CurrencyDto()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }
    }
}
