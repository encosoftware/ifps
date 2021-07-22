using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class UserTeamUpdateDtoValidator : AbstractValidator<UserTeamUpdateDto>
    {
        public UserTeamUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.UserIds).NotNull().NotEmpty();
        }
    }
}