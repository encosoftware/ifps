using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    class WorktopBoardMaterialService : IWorktopBoardMaterialService
    {
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IWorktopBoardMaterialRepository worktopBoardMaterialRepository;

        public WorktopBoardMaterialService(
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ILanguageRepository languageRepository,
            IWorktopBoardMaterialRepository worktopBoardMaterialRepository
            )
        {
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
            this.worktopBoardMaterialRepository = worktopBoardMaterialRepository;
        }

        public async Task CreateWorktopBoardMaterialsFromCsv(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);
            var currencies = currencyRepository.GetAllList();

            string[] lines = System.IO.File.ReadAllLines(url);
            var newGroupingCategories = new List<GroupingCategory>();

            var codes = new List<string>();

            foreach (string line in lines)
            {
                var worktopBoard = new WorktopBoardMaterial();
                string[] data = line.Split(';');

                if (data.Length != 6)
                {
                    continue;
                }

                if (!currencies.Any(ent => ent.Name.Equals(data[4])))
                {
                    continue;
                }

                var existingWorktopBoard = worktopBoardMaterialRepository.SingleOrDefault(ent => ent.Code == data[0]);
                if (existingWorktopBoard != null)
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
                        .SingleAsync(ent => ent.CategoryType == Enums.GroupingCategoryEnum.WorktopBoard && ent.ParentGroup.CategoryType == Enums.GroupingCategoryEnum.MaterialType);

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

                worktopBoard.Code = data[0];
                worktopBoard.Description = data[1];
                worktopBoard.Category = groupingCategory;
                worktopBoard.AddPrice(materialPrice);
                worktopBoard.HasFiberDirection = false;
                worktopBoard.Dimension = new Dimension(Double.Parse(data[5]), 2700.0, 38.0);
                worktopBoard.ImageId = Guid.Parse("15619300-fcf1-4b8c-8a24-3c5a2fd3e513");

                codes.Add(data[0]);

                await worktopBoardMaterialRepository.InsertAsync(worktopBoard);
            }
        }
    }
}
