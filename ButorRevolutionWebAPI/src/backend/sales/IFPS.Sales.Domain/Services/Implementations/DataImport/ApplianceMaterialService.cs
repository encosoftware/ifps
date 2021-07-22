using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    class ApplianceMaterialService : IApplianceMaterialService
    {
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IApplianceMaterialRepository applianceMaterialRepository;
        private readonly ICompanyRepository companyRepository;

        public ApplianceMaterialService(
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ILanguageRepository languageRepository,
            IApplianceMaterialRepository applianceMaterialRepository,
            ICompanyRepository companyRepository
            )
        {
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
            this.applianceMaterialRepository = applianceMaterialRepository;
            this.companyRepository = companyRepository;
        }

        public async Task CreateApplianceMaterialsFromCsv(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);
            var currencies = currencyRepository.GetAllList();

            string[] lines = System.IO.File.ReadAllLines(url);
            var newGroupingCategories = new List<GroupingCategory>();

            var codes = new List<string>();

            foreach (string line in lines)
            {
                var appliance = new ApplianceMaterial();
                string[] data = line.Split(';');
                if (data.Length != 9)
                {
                    continue;
                }
                var companyList = companyRepository.GetAllList(ent => ent.Name.Equals(data[0]));

                if (companyList.Count != 1)
                {
                    continue;
                }
                if (!currencies.Any(ent => ent.Name.Equals(data[5])))
                {
                    continue;
                }
                if (!currencies.Any(ent => ent.Name.Equals(data[7])))
                {
                    continue;
                }

                var existingAppliance = applianceMaterialRepository.SingleOrDefault(ent => ent.Code == data[2]);
                if (existingAppliance != null)
                {
                    continue;
                }

                if (codes.Any(ent => ent.Equals(data[2])))
                {
                    continue;
                }

                var groupingCategory = await groupingCategoryRepository.GetGroupingCategoryByName(data[8]);
                if (groupingCategory == null)
                {
                    groupingCategory = newGroupingCategories.SingleOrDefault(ent => ent.Translations.Any(t => t.GroupingCategoryName.Equals(data[8])));
                }
                if (groupingCategory == null)
                {
                    var parentGroupingCategory = await groupingCategoryRepository
                        .SingleAsync(ent => ent.CategoryType == Domain.Enums.GroupingCategoryEnum.Appliances
                        && ent.ParentGroup.CategoryType == Enums.GroupingCategoryEnum.MaterialType);

                    groupingCategory = new GroupingCategory(parentGroupingCategory);

                    foreach (var language in languageRepository.GetAllList())
                    {
                        groupingCategory.AddTranslation(new GroupingCategoryTranslation(data[8], language.LanguageType));
                    }

                    if (!newGroupingCategories.Any(ent => ent.Translations.Any(t => t.GroupingCategoryName.Equals(data[8]))))
                    {
                        newGroupingCategories.Add(groupingCategory);
                        await groupingCategoryRepository.InsertAsync(groupingCategory);
                    }
                }

                var company = companyList.First();
                appliance.BrandId = company.Id;
                appliance.Category = groupingCategory;
                appliance.HanaCode = data[1];
                appliance.Code = data[2];
                appliance.Description = data[3];
                appliance.ImageId = Guid.Parse("525955cb-f710-401b-bb30-3318e1cea414");

                appliance.SellPrice = new Price(Double.Parse(data[4]), currencies.Single(ent => ent.Name.Equals(data[5])).Id);

                var materialPrice = new MaterialPrice(new Price(Double.Parse(data[6]), currencies.Single(ent => ent.Name.Equals(data[7])).Id));
                appliance.AddPrice(materialPrice);

                codes.Add(data[2]);

                await applianceMaterialRepository.InsertAsync(appliance);
            }
        }
    }
}
