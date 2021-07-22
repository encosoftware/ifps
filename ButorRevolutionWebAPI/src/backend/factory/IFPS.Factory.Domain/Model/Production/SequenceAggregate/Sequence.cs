using System;
using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class Sequence : FullAuditedAggregateRoot
    {
        /// <summary>
        /// The optimal order of the execution.
        /// </summary>
        public int SuccessionNumber { get; set; }

        public virtual FurnitureComponent FurnitureComponent { get; set; }
        public Guid FurnitureComponentId { get; set; }

        protected Sequence()
        {

        }
        public Sequence(int successionNumber)
        {
            this.SuccessionNumber = successionNumber;
        }

        public virtual double GetDistance(double sizeX, double sizeY, double sizeZ = 1.0) { return 0; }

    }
}