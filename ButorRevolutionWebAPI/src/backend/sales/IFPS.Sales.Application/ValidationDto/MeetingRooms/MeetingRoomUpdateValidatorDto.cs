using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class MeetingRoomUpdateValidatorDto : AbstractValidator<MeetingRoomUpdateDto>
    {
        public MeetingRoomUpdateValidatorDto()
        {
            RuleFor(ent => ent.Name).NotNull().NotEmpty();
            RuleFor(ent => ent.Description).NotNull().NotEmpty();
        }
    }
}
