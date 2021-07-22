using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class ProductionProcess : FullAuditedAggregateRoot
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        public Order Order { get; set; }
        public Guid OrderId { get; private set; }

        /// <summary>
        /// Private list of the workers
        /// </summary>
        private List<ProcessWorker> _workers;
        public IEnumerable<ProcessWorker> Workers => _workers.AsReadOnly();

        private ProductionProcess()
        {
            _workers = new List<ProcessWorker>();
        }

        public ProductionProcess(Guid orderId, int planId) : this()
        {
            OrderId = orderId;
            PlanId = planId;
        }

        public void AddWorkers(ProcessWorker worker)
        {
            Ensure.NotNull(worker);
            _workers.Add(worker);
        }
    }
}
