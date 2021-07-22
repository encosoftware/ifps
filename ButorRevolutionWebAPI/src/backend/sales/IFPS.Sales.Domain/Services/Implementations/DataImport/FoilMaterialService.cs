using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    class FoilMaterialService : IFoilMaterialService
    {
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IFoilMaterialRepository foilMaterialRepository;

        public FoilMaterialService(
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ILanguageRepository languageRepository,
            IFoilMaterialRepository foilMaterialRepository
            )
        {
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
            this.foilMaterialRepository = foilMaterialRepository;
        }

        public async Task CreateFoilMaterialsFromCsv(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);
            var currencies = currencyRepository.GetAllList();

            string[] lines = System.IO.File.ReadAllLines(url);
            var newGroupingCategories = new List<GroupingCategory>();

            var codes = new List<string>();

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (data.Length != 6)
                {
                    continue;
                }

                if (!currencies.Any(ent => ent.Name.Equals(data[4])))
                {
                    continue;
                }

                var existingFoil = foilMaterialRepository.SingleOrDefault(ent => ent.Code == data[0]);
                if (existingFoil != null)
                {
                    continue;
                }

                if (codes.Any(ent => ent.Equals(data[0])))
                {
                    continue;
                }

                var groupingCategory = await groupingCategoryRepository.GetGroupingCategoryByName(data[2]);
                if (groupingCategory == null)
                {
                    groupingCategory = newGroupingCategories.SingleOrDefault(ent => ent.Translations.Any(t => t.GroupingCategoryName.Equals(data[2])));
                }
                if (groupingCategory == null)
                {
                    var parentGroupingCategory = await groupingCategoryRepository
                        .SingleAsync(ent => ent.CategoryType == Domain.Enums.GroupingCategoryEnum.FoilMaterial
                        && ent.ParentGroup.CategoryType == Enums.GroupingCategoryEnum.MaterialType);

                    groupingCategory = new GroupingCategory(parentGroupingCategory);

                    foreach (var language in languageRepository.GetAllList())
                    {
                        groupingCategory.AddTranslation(new GroupingCategoryTranslation(data[2], language.LanguageType));
                    }

                    if (!newGroupingCategories.Any(ent => ent.Translations.Any(t => t.GroupingCategoryName.Equals(data[2]))))
                    {
                        newGroupingCategories.Add(groupingCategory);
                        await groupingCategoryRepository.InsertAsync(groupingCategory);
                    }
                }

                var materialPrice = new MaterialPrice(new Price(Double.Parse(data[3]), currencies.Single(ent => ent.Name.Equals(data[4])).Id));

                var foil = new FoilMaterial();
                foil.Code = data[0];
                foil.Description = data[1];
                foil.AddPrice(materialPrice);
                foil.Category = groupingCategory;
                foil.ImageId = Guid.Parse("07a316ed-48df-4dcc-919f-170224574cd2");
                var thicknessStr = data[5].Replace(',', '.');
                foil.Thickness = Double.Parse(thicknessStr, CultureInfo.InvariantCulture);

                codes.Add(data[0]);

                await foilMaterialRepository.InsertAsync(foil);
            }
        }
    }
}
