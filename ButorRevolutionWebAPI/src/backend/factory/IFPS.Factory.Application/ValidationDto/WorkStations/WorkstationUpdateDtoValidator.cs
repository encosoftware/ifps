using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto.WorkStations
{
    public class WorkstationUpdateDtoValidator : AbstractValidator<WorkStationUpdateDto>
    {
        public WorkstationUpdateDtoValidator()
        {
            RuleFor(ent => ent.Name).NotNull().NotEmpty();
            RuleFor(ent => ent.OptimalCrew).NotNull().NotEmpty();
            RuleFor(ent => ent.MachineId).NotNull().GreaterThan(0);
            RuleFor(ent => ent.WorkStationTypeId).NotNull().GreaterThan(0);
        }
    }
}
