using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/generalexpenses")]
    [ApiController]
    public class GeneralExpenseController : IFPSControllerBase
    {
        private const string OPNAME = "GeneralExpenses";
        private readonly IGeneralExpenseAppService service;

        public GeneralExpenseController(IGeneralExpenseAppService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "UpdateGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateGeneralExpense([FromBody]GeneralExpenseCreateDto generalExpenseCreateDto)
        {
            return service.CreateGeneralExpenseAsync(generalExpenseCreateDto);
        }

        [HttpPost("rules")]
        [Authorize(Policy = "UpdateGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateGeneralExpenseRule([FromBody]GeneralExpenseRuleCreateDto generalExpenseRuleCreateDto)
        {
            return service.CreateGeneralExpenseRuleAsync(generalExpenseRuleCreateDto);
        }

        [HttpGet]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<GeneralExpenseListDto>> ListGeneralExpenses([FromQuery]GeneralExpenseFilterDto generalExpenseFilterDto)
        {
            return service.GetGeneralExpensesAsync(generalExpenseFilterDto);
        }

        [HttpGet("download/expenses")]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DownloadFilteredExpensesResults([FromQuery] GeneralExpenseFilterDto generalExpenseFilterDto)
        {
            await service.ExportExpensesResults(generalExpenseFilterDto);
        }

        [HttpGet("generalrules")]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<GeneralExpenseRuleListDto>> ListGeneralExpenseRules([FromQuery]GeneralExpenseRuleFilterDto generalExpenseRuleFilterDto)
        {
            return service.GetGeneralExpenseRulesAsync(generalExpenseRuleFilterDto);
        }

        [HttpGet("download/rules")]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DownloadFilteredRulesResults([FromQuery] GeneralExpenseRuleFilterDto generalExpenseRuleFilterDto)
        {
            await service.DownloadRulesResultsAsync(generalExpenseRuleFilterDto);
        }

        [HttpGet("{expenseId}")]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<GeneralExpenseDetailsDto> GetGeneralExpense(int expenseId)
        {
            return service.GetGeneralExpenseAsync(expenseId);
        }

        [HttpGet("rules/{expenseRuleId}")]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<GeneralExpenseRuleDetailsDto> GetGeneralExpenseRule(int expenseRuleId)
        {
            return service.GetGeneralExpenseRuleAsync(expenseRuleId);
        }

        [HttpGet("recurring")]
        [Authorize(Policy = "GetGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<RecurrentExpenseListDto>> GetMonthlyRecurringCostAsync()
        {
            return service.GetMonthlyRecurringCostAsync();
        }

        [HttpPut("{expenseId}")]
        [Authorize(Policy = "UpdateGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateGeneralExpense(int expenseId, [FromBody] GeneralExpenseUpdateDto generalExpenseUpdateDto)
        {
            return service.UpdateGeneralExpenseAsync(expenseId, generalExpenseUpdateDto);
        }

        [HttpPut("rules/{expenseRuleId}")]
        [Authorize(Policy = "UpdateGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateGeneralExpenseRule(int expenseRuleId, [FromBody] GeneralExpenseRuleUpdateDto generalExpenseRuleUpdateDto)
        {
            return service.UpdateGeneralExpenseRuleAsync(expenseRuleId, generalExpenseRuleUpdateDto);
        }

        [HttpPut("recurring")]
        [Authorize(Policy = "UpdateGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateMonthlyRecurringCostsAsync([FromBody] List<RecurrentExpenseUpdateDto> recurrentExpenseUpdateDtos)
        {
            return service.UpdateMonthlyRecurringCostsAsync(recurrentExpenseUpdateDtos);
        }

        [HttpDelete("{expenseId}")]
        [Authorize(Policy = "DeleteGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteGeneralExpense(int expenseId)
        {
            return service.DeleteGeneralExpenseAsync(expenseId);
        }

        [HttpDelete("rules/{expenseRuleId}")]
        [Authorize(Policy = "DeleteGeneralExpenses")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteGeneralExpenseRule(int expenseRuleId)
        {
            return service.DeleteGeneralExpenseRuleAsync(expenseRuleId);
        }
    }
}
