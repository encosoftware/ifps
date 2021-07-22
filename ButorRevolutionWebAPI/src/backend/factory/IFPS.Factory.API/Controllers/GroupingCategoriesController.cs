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
    [Route("api/groupingcategories")]
    [ApiController]
    public class GroupingCategoriesController : IFPSControllerBase
    {
        private const string OPNAME = "GroupingCategories";

        private readonly IGroupingCategoryAppService groupingCategoryAppService;

        public GroupingCategoriesController(
            IGroupingCategoryAppService groupingCategoryAppService)
        {
            this.groupingCategoryAppService = groupingCategoryAppService;
        }

        // GET grouping categories root list
        [HttpGet("flatList")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingCategoryListDto>> GetRootCategories([FromQuery]GroupingCategoryFilterDto filter)
        {
            return groupingCategoryAppService.GetFlatGroupingCategoriesAsync(filter);
        }

        // GET grouping categories hierarchical list
        [HttpGet("hierarchicalList")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingCategoryListDto>> GetHierarchicalCategories([FromQuery]GroupingCategoryFilterDto filter)
        {
            return groupingCategoryAppService.GetHierarchicalGroupingCategoriesAsync(filter);
        }

        //GET grouping category by id
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<GroupingCategoryDetailsDto> GetById(int id)
        {
            return groupingCategoryAppService.GetGroupingCategoryDetailsAsync(id);
        }
    }
}