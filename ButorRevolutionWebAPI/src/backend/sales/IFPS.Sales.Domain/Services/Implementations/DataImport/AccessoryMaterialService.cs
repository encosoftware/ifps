using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    class AccessoryMaterialService : IAccessoryMaterialService
    {
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IAccessoryMaterialRepository accessoryMaterialRepository;

        public AccessoryMaterialService(
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ILanguageRepository languageRepository,
            IAccessoryMaterialRepository accessoryMaterialRepository
            )
        {
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
            this.accessoryMaterialRepository = accessoryMaterialRepository;
        }

        public async Task CreateAccessoryMaterialsFromCsv(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);
            var currencies = currencyRepository.GetAllList();

            string[] lines = System.IO.File.ReadAllLines(url);
            var newGroupingCategories = new List<GroupingCategory>();

            var codes = new List<string>();

            foreach (string line in lines)
            {

                string[] data = line.Split(';');

                if (data.Length != 7)
                {
                    continue;
                }

                if (!currencies.Any(ent => ent.Name.Equals(data[6])))
                {
                    continue;
                }

                var existingAccessory = accessoryMaterialRepository.SingleOrDefault(ent => ent.Code == data[0]);
                if (existingAccessory != null)
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
                        .SingleAsync(ent => ent.CategoryType == Enums.GroupingCategoryEnum.Accessories
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

                var materialPrice = new MaterialPrice(new Price(Double.Parse(data[5]), currencies.Single(ent => ent.Name.Equals(data[6])).Id));

                int isOptionalInt = int.Parse(data[4]);
                bool isOptional = isOptionalInt == 1 ? true : false;
                int isRequiredForAssemblyInt = int.Parse(data[5]);
                bool isRequiredForAssembly = isRequiredForAssemblyInt == 1 ? true : false;

                var accessory = new AccessoryMaterial(isOptional, isRequiredForAssembly, data[0]);
                accessory.Description = data[1];
                accessory.Category = groupingCategory;
                accessory.AddPrice(materialPrice);
                accessory.ImageId = Guid.Parse("d647832c-02a2-475e-a154-fb60b5f45d6e");

                codes.Add(data[0]);

                await accessoryMaterialRepository.InsertAsync(accessory);
            }
        }
    }
}
