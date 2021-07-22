using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
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
