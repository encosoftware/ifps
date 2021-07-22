using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
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