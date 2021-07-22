using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class GroupingCategoryFilterDtoValidation : AbstractValidator<GroupingCategoryFilterDto>
    {
        public GroupingCategoryFilterDtoValidation()
        {
        }
    }
}