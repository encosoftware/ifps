using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class SelectedRequiredMaterialsDto
    {
        public List<int> RequiredMaterialsIds { get; set; }
        public int SupplierId { get; set; }
        public int BookedById { get; set; }
    }
}
