using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Plan : FullAuditedAggregateRoot
    {
        public DateTime ScheduledStartTime { get; set; }
        public DateTime ScheduledEndTime { get; set; }

        public virtual WorkStation WorkStation { get; set; }
        public int? WorkStationId { get; set; }

        public virtual Optimization Optimization { get; set; }
        public Guid OptimizationId { get; set; }
        
        public virtual ProductionProcess ProductionProcess { get; set; }
        public int? ProductionProcessId { get; set; }

        public virtual ConcreteFurnitureComponent ConcreteFurnitureComponent { get; set; }
        public int? ConcreteFurnitureComponentId { get; set; }

        public virtual ConcreteFurnitureUnit ConcreteFurnitureUnit { get; set; }
        public int? ConcreteFurnitureUnitId { get; set; }

        public Plan()
        {

        }
    }
}
