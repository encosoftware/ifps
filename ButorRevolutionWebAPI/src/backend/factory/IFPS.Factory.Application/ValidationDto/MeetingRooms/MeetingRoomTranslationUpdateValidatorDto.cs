using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class MeetingRoomTranslationUpdateValidatorDto : AbstractValidator<MeetingRoomTranslationUpdateDto>
    {
        public MeetingRoomTranslationUpdateValidatorDto()
        {
            RuleFor(ent => ent.Description).NotNull().NotEmpty();
            RuleFor(ent => ent.Name).NotNull().NotEmpty();
            RuleFor(ent => ent.Language).NotNull().NotEmpty();
        }
    }
}
