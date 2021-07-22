using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class ImageCreateDtoValidator : AbstractValidator<ImageCreateDto>
    {
        public ImageCreateDtoValidator()
        {
            RuleFor(x => x.ContainerName).NotNull().NotEmpty();
            RuleFor(x => x.FileName).NotNull().NotEmpty();
        }
    }
}