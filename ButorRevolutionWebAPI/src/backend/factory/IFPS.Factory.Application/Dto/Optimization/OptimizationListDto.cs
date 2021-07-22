using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class OptimizationListDto
    {
        public Guid Id { get; set; }
        public int ShiftNumber { get; set; }
        public int ShiftLength { get; set; }
        public DateTime StartedAt { get; set; }

        public static Func<Optimization, OptimizationListDto> FromEntity = entity => new OptimizationListDto()
        {
            Id = entity.Id,
            ShiftLength = entity.ShiftLength,
            ShiftNumber = entity.ShiftNumber,
            StartedAt = entity.CreationTime,
        };
    }
}
