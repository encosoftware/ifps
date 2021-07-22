using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IGeneralExpenseAppService
    {
        Task<int> CreateGeneralExpenseAsync(GeneralExpenseCreateDto generalExpenseCreateDto);
        Task<int> CreateGeneralExpenseRuleAsync(GeneralExpenseRuleCreateDto generalExpenseRuleCreateDto);
        Task<PagedListDto<GeneralExpenseListDto>> GetGeneralExpensesAsync(GeneralExpenseFilterDto dto);
        Task<PagedListDto<GeneralExpenseRuleListDto>> GetGeneralExpenseRulesAsync(GeneralExpenseRuleFilterDto dto);
        Task<GeneralExpenseDetailsDto> GetGeneralExpenseAsync(int expenseId);
        Task<GeneralExpenseRuleDetailsDto> GetGeneralExpenseRuleAsync(int expenseRuleId);
        Task<List<RecurrentExpenseListDto>> GetMonthlyRecurringCostAsync();
        Task UpdateMonthlyRecurringCostsAsync(List<RecurrentExpenseUpdateDto> recurrentExpenseUpdateDtos);
        Task UpdateGeneralExpenseAsync(int expenseId, GeneralExpenseUpdateDto generalExpenseUpdateDto);
        Task UpdateGeneralExpenseRuleAsync(int expenseRuleId, GeneralExpenseRuleUpdateDto generalExpenseRuleUpdateDto);
        Task DeleteGeneralExpenseAsync(int expenseId);
        Task DeleteGeneralExpenseRuleAsync(int expenseRuleId);
        Task ExportExpensesResults(GeneralExpenseFilterDto generalExpenseFilterDto);
        Task DownloadRulesResultsAsync(GeneralExpenseRuleFilterDto generalExpenseRuleFilterDto);
    }
}
