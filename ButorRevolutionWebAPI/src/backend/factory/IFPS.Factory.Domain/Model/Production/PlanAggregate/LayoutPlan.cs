using ENCO.DDD;
using IFPS.Factory.Domain.Enums;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class LayoutPlan : Plan
    {
        private List<Cutting> _cuttings;
        public IEnumerable<Cutting> Cuttings => _cuttings.AsReadOnly();

        public virtual BoardMaterial Board { get; set; }
        public Guid? BoardId { get; set; }

        public LayoutPlanStatusEnum Status { get; set; }

        public LayoutPlan() : base()
        {
            _cuttings = new List<Cutting>();
        }

        public LayoutPlan(Guid? boardId) : this()
        {
            this.BoardId = boardId;
        }

        public void AddCutting(Cutting cutting)
        {
            Ensure.NotNull(cutting);
            _cuttings.Add(cutting);
        }
    }
}
