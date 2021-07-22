using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
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
            var countries = await countryRepository.GetAllListAsync();

            return countries.Select(ent => new CountryListDto(ent)).ToList();
        }
    }
}
