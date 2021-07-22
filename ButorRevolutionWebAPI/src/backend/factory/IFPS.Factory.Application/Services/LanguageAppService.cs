using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class LanguageAppService : ApplicationService, ILanguageAppService
    {
        private readonly ILanguageRepository languageRepository;
        public LanguageAppService(IApplicationServiceDependencyAggregate aggregate
            , ILanguageRepository languageRepository) : base(aggregate)
        {
            this.languageRepository = languageRepository;
        }

        public async Task<List<LanguageListDto>> GetLanguages()
        {
            var languages = await languageRepository.GetAllListIncludingAsync(x => true, ent => ent.Translations);
            return languages.Select(x => new LanguageListDto
            {
                LanguageType = x.LanguageType,
                Translation = x.CurrentTranslation.LanguageName,
            }).ToList();
        }
    }
}
