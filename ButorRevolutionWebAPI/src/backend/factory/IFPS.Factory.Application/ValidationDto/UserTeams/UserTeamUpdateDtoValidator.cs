using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
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
