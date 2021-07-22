using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureRecommendationDto
    {
        public List<Guid> Ids { get; set; }

        public FurnitureRecommendationDto()
        {
            Ids = new List<Guid>();
        }

        public FurnitureRecommendationDto(List<Guid> ids) : this()
        {
            Ids = ids;
        }
    }
}
