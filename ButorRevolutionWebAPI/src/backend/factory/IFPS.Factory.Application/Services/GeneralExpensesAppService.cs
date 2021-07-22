using CsvHelper;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class GeneralExpensesAppService : ApplicationService, IGeneralExpenseAppService
    {

        private readonly IGeneralExpenseRuleRepository generalExpenseRuleRepository;
        private readonly IGeneralExpenseRepository generalExpenseRepository;
        private readonly IFileHandlerService fileHandlerService;

        public GeneralExpensesAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IGeneralExpenseRuleRepository generalExpenseRuleRepository,
            IGeneralExpenseRepository generalExpenseRepository,
            IHostingEnvironment hostingEnvironment,
            IFileHandlerService fileHandlerService) : base(aggregate)
        {
            this.generalExpenseRuleRepository = generalExpenseRuleRepository;
            this.generalExpenseRepository = generalExpenseRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<int> CreateGeneralExpenseRuleAsync(GeneralExpenseRuleCreateDto generalExpenseRuleCreateDto)
        {
            var generalExpenseRule = generalExpenseRuleCreateDto.CreateModelObject();
            await generalExpenseRuleRepository.InsertAsync(generalExpenseRule);
            await unitOfWork.SaveChangesAsync();

            return generalExpenseRule.Id;
        }

        public async Task<int> CreateGeneralExpenseAsync(GeneralExpenseCreateDto generalExpenseCreateDto)
        {
            var generalExpense = generalExpenseCreateDto.CreateModelObject();
            await generalExpenseRepository.InsertAsync(generalExpense);
            await unitOfWork.SaveChangesAsync();

            return generalExpense.Id;
        }

        public async Task DeleteGeneralExpenseRuleAsync(int generalExpenseRuleId)
        {
            await generalExpenseRuleRepository.DeleteAsync(generalExpenseRuleId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteGeneralExpenseAsync(int generalExpenseId)
        {
            await generalExpenseRepository.DeleteAsync(generalExpenseId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<GeneralExpenseRuleDetailsDto> GetGeneralExpenseRuleAsync(int generalExpenseRuleId)
        {
            var generalExpenseRule = await generalExpenseRuleRepository.SingleIncludingAsync(ent => ent.Id == generalExpenseRuleId, ent => ent.Amount.Currency);
            return new GeneralExpenseRuleDetailsDto(generalExpenseRule);
        }


        public async Task<GeneralExpenseDetailsDto> GetGeneralExpenseAsync(int generalExpenseId)
        {
            var generalExpense = await generalExpenseRepository.SingleIncludingAsync(ent => ent.Id == generalExpenseId, ent => ent.GeneralExpenseRule, ent => ent.Cost.Currency);
            return new GeneralExpenseDetailsDto(generalExpense);
        }

        public async Task<PagedListDto<GeneralExpenseListDto>> GetGeneralExpensesAsync(GeneralExpenseFilterDto generalExpenseFilterDto)
        {
            Expression<Func<GeneralExpense, bool>> filter = CreateExpensesFilter(generalExpenseFilterDto);

            var orderingQuery = generalExpenseFilterDto.Orderings.ToOrderingExpression<GeneralExpense>(
                GeneralExpenseFilterDto.GetOrderingMapping(), nameof(GeneralExpense.Id));

            var generalExpenses = await generalExpenseRepository.GetGeneralExpensesWithIncludesAsync(
                filter, orderingQuery, generalExpenseFilterDto.PageIndex, generalExpenseFilterDto.PageSize);

            return generalExpenses.ToPagedList(GeneralExpenseListDto.FromEntity);
        }

        private static Expression<Func<GeneralExpense, bool>> CreateExpensesFilter(GeneralExpenseFilterDto generalExpenseFilterDto)
        {
            Expression<Func<GeneralExpense, bool>> filter = (GeneralExpense ent) => true;

            if (!string.IsNullOrWhiteSpace(generalExpenseFilterDto.Name))
            {
                filter = filter.And(e => e.GeneralExpenseRule.Name.ToLower().Contains(generalExpenseFilterDto.Name.ToLower().Trim()));
            }

            if (generalExpenseFilterDto.Amount != 0)
            {
                filter = filter.And(e => e.Cost.Value.Equals(generalExpenseFilterDto.Amount));
            }
            if (generalExpenseFilterDto.PaymentDateFrom.HasValue)
            {
                filter = filter.And(ent => ent.PaymentDate >= generalExpenseFilterDto.PaymentDateFrom);
            }
            if (generalExpenseFilterDto.PaymentDateTo.HasValue)
            {
                filter = filter.And(ent => ent.PaymentDate <= generalExpenseFilterDto.PaymentDateTo);
            }

            return filter;
        }

        public async Task<PagedListDto<GeneralExpenseRuleListDto>> GetGeneralExpenseRulesAsync(GeneralExpenseRuleFilterDto generalExpenseRuleFilterDto)
        {
            Expression<Func<GeneralExpenseRule, bool>> filter = CreateRulesFilter(generalExpenseRuleFilterDto);

            var orderingQuery = generalExpenseRuleFilterDto.Orderings.ToOrderingExpression<GeneralExpenseRule>(
                GeneralExpenseRuleFilterDto.GetOrderingMapping(), nameof(GeneralExpenseRule.Id));

            var generalExpenses = await generalExpenseRuleRepository.GetGeneralExpenseRulesWithIncludesAsync(
                filter, orderingQuery, generalExpenseRuleFilterDto.PageIndex, generalExpenseRuleFilterDto.PageSize);

            return generalExpenses.ToPagedList(GeneralExpenseRuleListDto.FromEntity);
        }

        private static Expression<Func<GeneralExpenseRule, bool>> CreateRulesFilter(GeneralExpenseRuleFilterDto generalExpenseRuleFilterDto)
        {
            Expression<Func<GeneralExpenseRule, bool>> filter = (GeneralExpenseRule ent) => ent.Name != null;

            if (!string.IsNullOrWhiteSpace(generalExpenseRuleFilterDto.Name))
            {
                filter = filter.And(e => e.Name.ToLower().Contains(generalExpenseRuleFilterDto.Name.ToLower().Trim()));
            }

            if (generalExpenseRuleFilterDto.Amount != 0)
            {
                filter = filter.And(e => e.Amount.Value.Equals(generalExpenseRuleFilterDto.Amount));
            }

            if (generalExpenseRuleFilterDto.FrequencyFrom != 0)
            {
                filter = filter.And(e => e.Interval > generalExpenseRuleFilterDto.FrequencyFrom && e.FrequencyTypeId == generalExpenseRuleFilterDto.FrequencyTypeId);
            }

            if (generalExpenseRuleFilterDto.FrequencyTo != 0)
            {
                filter = filter.And(e => e.Interval < generalExpenseRuleFilterDto.FrequencyTo && e.FrequencyTypeId == generalExpenseRuleFilterDto.FrequencyTypeId);
            }

            if (generalExpenseRuleFilterDto.StartDateFrom.HasValue)
            {
                filter = filter.And(ent => ent.StartDate >= generalExpenseRuleFilterDto.StartDateFrom);
            }
            if (generalExpenseRuleFilterDto.StartDateTo.HasValue)
            {
                filter = filter.And(ent => ent.StartDate <= generalExpenseRuleFilterDto.StartDateTo);
            }

            return filter;
        }

        public async Task<List<RecurrentExpenseListDto>> GetMonthlyRecurringCostAsync()
        {
            Expression<Func<GeneralExpenseRule, bool>> filter = (GeneralExpenseRule ent) => ent.ExpenseType == GeneralExpenseRuleEnum.RecurrentVariable;
            filter = filter.Or(ent => ent.FrequencyType.Type == FrequencyTypeEnum.Day && ent.LastPaymentDate.AddDays(ent.Interval) < Clock.Now.AddMonths(1));
            filter = filter.Or(ent => ent.FrequencyType.Type == FrequencyTypeEnum.Week && ent.LastPaymentDate.AddDays(ent.Interval * 7) < Clock.Now.AddMonths(1));
            filter = filter.Or(ent => ent.FrequencyType.Type == FrequencyTypeEnum.Month && ent.LastPaymentDate.AddMonths(ent.Interval) < Clock.Now.AddMonths(1));
            filter = filter.Or(ent => ent.FrequencyType.Type == FrequencyTypeEnum.Year && ent.LastPaymentDate.AddYears(ent.Interval) < Clock.Now.AddMonths(1));

            var generalExpenseRules = await generalExpenseRuleRepository.GetAllListIncludingAsync(filter, ent => ent.Amount.Currency);
            return generalExpenseRules.Select(ent => new RecurrentExpenseListDto(ent)).ToList();
        }

        public async Task UpdateGeneralExpenseRuleAsync(int expenseRuleId, GeneralExpenseRuleUpdateDto generalExpenseRuleUpdateDto)
        {
            var generalExpenseRule = await generalExpenseRuleRepository.SingleIncludingAsync(ent => ent.Id == expenseRuleId, ent => ent.FrequencyType);
            generalExpenseRule = generalExpenseRuleUpdateDto.UpdateModelObject(generalExpenseRule);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateGeneralExpenseAsync(int expenseId, GeneralExpenseUpdateDto generalExpenseUpdateDto)
        {
            var generalExpense = await generalExpenseRepository.SingleIncludingAsync(ent => ent.Id == expenseId, ent => ent.GeneralExpenseRule, ent => ent.GeneralExpenseRule.FrequencyType);
            generalExpense = generalExpenseUpdateDto.UpdateModelObject(generalExpense);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateMonthlyRecurringCostsAsync(List<RecurrentExpenseUpdateDto> recurrentExpenseUpdateDtos)
        {
            var idList = new List<int>();
            recurrentExpenseUpdateDtos.ForEach(ent => idList.Add(ent.Id));
            var generalExpenseRules = await generalExpenseRuleRepository.GetAllListAsync(ent => idList.Contains(ent.Id));
            generalExpenseRules.ForEach(ent => ent = recurrentExpenseUpdateDtos.Single(dto => dto.Id == ent.Id).UpdateModelObject(ent));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ExportExpensesResults(GeneralExpenseFilterDto generalExpenseFilterDto)
        {
            Expression<Func<GeneralExpense, bool>> filter = CreateExpensesFilter(generalExpenseFilterDto);
            List<GeneralExpense> generalExpenses =
                await generalExpenseRepository.GetAllListIncludingAsync(filter, ent => ent.GeneralExpenseRule, ent => ent.Cost);

            using (var stream = new System.IO.MemoryStream())
            {
                using (var writer = new System.IO.StreamWriter(stream))
                {
                    using (var csvWriter = new CsvWriter(writer))
                    {
                        csvWriter.Configuration.Delimiter = ";";

                        csvWriter.WriteField("Amount");
                        csvWriter.WriteField("Name");
                        csvWriter.WriteField("Date");

                        await csvWriter.NextRecordAsync();

                        foreach (var ge in generalExpenses)
                        {
                            csvWriter.WriteField(ge.GeneralExpenseRule.Amount.Value);
                            csvWriter.WriteField(ge.GeneralExpenseRule.Name);
                            csvWriter.WriteField(ge.PaymentDate);

                            await csvWriter.NextRecordAsync();
                        }

                        await writer.FlushAsync();
                        stream.Position = 0;

                        await fileHandlerService.UploadFromStreamAsync(stream, FileContainerProvider.Containers.Temp, ".csv");
                    }
                }
            }
        }

        public async Task DownloadRulesResultsAsync(GeneralExpenseRuleFilterDto generalExpenseRuleFilterDto)
        {
            Expression<Func<GeneralExpenseRule, bool>> filter = CreateRulesFilter(generalExpenseRuleFilterDto);

            List<GeneralExpenseRule> generalExpenseRules =
              await generalExpenseRuleRepository.GetAllListIncludingAsync(filter, ent => ent.FrequencyType, ent => ent.Amount, ent => ent.FrequencyType.Translations);

            using (var stream = new System.IO.MemoryStream())
            {
                using (var writer = new System.IO.StreamWriter(stream))
                {
                    using (var csvWriter = new CsvWriter(writer))
                    {
                        csvWriter.Configuration.Delimiter = ";";

                        csvWriter.WriteField("Amount");
                        csvWriter.WriteField("Name");
                        csvWriter.WriteField("Frequency");
                        csvWriter.WriteField("Start date");

                        await csvWriter.NextRecordAsync();

                        foreach (var gu in generalExpenseRules)
                        {
                            csvWriter.WriteField(gu.Amount.Value);
                            csvWriter.WriteField(gu.Name);
                            csvWriter.WriteField(gu.FrequencyType.Translations.GetCurrentTranslation().Name);
                            csvWriter.WriteField(gu.StartDate);

                            await csvWriter.NextRecordAsync();
                        }

                        await writer.FlushAsync();
                        stream.Position = 0;

                        await fileHandlerService.UploadFromStreamAsync(stream, FileContainerProvider.Containers.Temp, ".csv");
                    }
                }
            }
        }
    }
}

