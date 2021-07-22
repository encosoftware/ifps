using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
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
        [Authorize(Policy = "GetGroupingCategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingCategoryListDto>> GetRootCategories([FromQuery]GroupingCategoryFilterDto filter)
        {
            return groupingCategoryAppService.GetFlatGroupingCategoriesAsync(filter);
        }

        // GET grouping categories root list
        [HttpGet("dropdown")]
        [Authorize()]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingCategoryListDto>> GetCategoriesForDropdown([FromQuery]GroupingCategoryFilterDto filter)
        {
            return groupingCategoryAppService.GetCategoriesForDropdownAsync(filter.CategoryType);
        }

        // GET grouping categories hierarchical list
        [HttpGet("hierarchicalList")]
        [Authorize(Policy = "GetGroupingCategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingCategoryListDto>> GetHierarchicalCategories([FromQuery]GroupingCategoryFilterDto filter)
        {
            return groupingCategoryAppService.GetHierarchicalGroupingCategoriesAsync(filter);
        }

        //GET grouping category by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetGroupingCategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<GroupingCategoryDetailsDto> GetById(int id)
        {
            return groupingCategoryAppService.GetGroupingCategoryDetailsAsync(id);
        }

        // POST: api/groupingCategories
        [HttpPost]
        [Authorize(Policy = "UpdateGroupingCategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> Post([FromBody] GroupingCategoryCreateDto createDto)
        {
            return groupingCategoryAppService.CreateGroupingCategoryAsync(createDto);
        }

        // PUT: api/groupingCategory/
        [HttpPut("{id}")]
        //[Authorize(Policy = "UpdateGroupingCategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateGroupingCategory(int id, [FromBody]GroupingCategoryUpdateDto updateDto)
        {
            return groupingCategoryAppService.UpdateGroupingCategoryAsync(id, updateDto);
        }

        //DELETE: api/groupingCategories/
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteGroupingCategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteGroupingCategory(int id)
        {
            return groupingCategoryAppService.DeleteGroupingCategoryAsync(id);
        }

        [HttpGet("webshop")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingCategoryWebshopListDto>> GetCategoriesByWebshop()
        {
            return groupingCategoryAppService.GetFurnitureUnitGroupingCategoriesForWebshopAsync();
        }

        [HttpGet("{categoryId}/webshop/subcategories")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<GroupingSubCategoryWebshopListDto>> GetSubcategeroiesByCategory(int categoryId)
        {
            return groupingCategoryAppService.GetSubcategeroiesByCategoryAsync(categoryId);
        }

        // GET categories dropdown by offer form general information page
        [HttpGet("decorboards/dropdown")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DecorBoardGroupingCategoryListDto>> GetDecorBoardCategories()
        {
            return groupingCategoryAppService.GetDecorBoardCategoriesAsync();
        }
    }
}