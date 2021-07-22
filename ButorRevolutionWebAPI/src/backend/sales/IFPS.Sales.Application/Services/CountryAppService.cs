using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class CountryAppService : ApplicationService, ICountryAppService
    {
        private readonly ICountryRepository countryRepository;

        public CountryAppService(IApplicationServiceDependencyAggregate aggregate,
            ICountryRepository countryRepository)
            : base(aggregate)
        {
            this.countryRepository = countryRepository;
        }

        public async Task<List<CountryListDto>> GetCountriesAsync()
        {
            return (await countryRepository.GetAllListAsync()).Select(ent => new CountryListDto(ent)).ToList();
        }
    }
}
