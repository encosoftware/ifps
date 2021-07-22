using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CFCProductionStateListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CFCProductionStateListDto(CFCProductionState cfcProductionState)
        {
            Id = cfcProductionState.Id;
            Name = cfcProductionState.CurrentTranslation.Name;
        }
    }
}
