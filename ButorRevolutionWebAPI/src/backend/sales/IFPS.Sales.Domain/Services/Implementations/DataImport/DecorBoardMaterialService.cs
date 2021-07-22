using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    class DecorBoardMaterialService : IDecorBoardMaterialService
    {
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IDecorBoardMaterialRepository decorBoardMaterialRepository;

        public DecorBoardMaterialService(
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ILanguageRepository languageRepository,
            IDecorBoardMaterialRepository decorBoardMaterialRepository
            )
        {
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
            this.decorBoardMaterialRepository = decorBoardMaterialRepository;
        }

        public async Task CreateDecorBoardMaterialsFromCsv(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);
            var currencies = currencyRepository.GetAllList();

            string[] lines = System.IO.File.ReadAllLines(url);
            var newGroupingCategories = new List<GroupingCategory>();

            var codes = new List<string>();

            foreach (string line in lines)
            {
                var decorBoard = new DecorBoardMaterial();
                string[] data = line.Split(';');

                if (data.Length != 9)
                {
                    continue;
                }

                if (!currencies.Any(ent => ent.Name.Equals(data[4])))
                {
                    continue;
                }

                if (data[3] == "")
                {
                    continue;
                }

                var existingDecorBoard = decorBoardMaterialRepository.SingleOrDefault(ent => ent.Code == data[0]);
                if (existingDecorBoard != null)
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
                        .SingleAsync(ent => ent.CategoryType == Enums.GroupingCategoryEnum.DecorBoard
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

                decorBoard.Code = data[0];
                decorBoard.Description = data[1];
                decorBoard.Category = groupingCategory;
                decorBoard.AddPrice(materialPrice);
                int fiberDirection = int.Parse(data[8]);
                decorBoard.HasFiberDirection = fiberDirection == 1 ? true : false;
                decorBoard.Dimension = new Dimension(Double.Parse(data[6]), Double.Parse(data[5]), Double.Parse(data[7]));
                decorBoard.ImageId = Guid.Parse("c1882142-743e-41e5-a2c2-d62ba7a7cba4");

                codes.Add(data[0]);

                await decorBoardMaterialRepository.InsertAsync(decorBoard);
            }
        }
    }
}
