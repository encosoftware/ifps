using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class ImageUpdateDtoValidator : AbstractValidator<ImageUpdateDto>
    {
        public ImageUpdateDtoValidator()
        {
            RuleFor(x => x.ContainerName).NotNull().NotEmpty();
            RuleFor(x => x.FileName).NotNull().NotEmpty();
        }
    }
}