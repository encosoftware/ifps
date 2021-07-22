using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class Optimization : FullAuditedAggregateRoot<Guid>
    {
        public int ShiftNumber { get; set; }
        public int ShiftLength { get; set; }

        public LayoutPlanZipFile LayoutPlanZip { get; set; }
        public Guid LayoutPlanZipId { get; set; }
        
        public ScheduleZipFile ScheduleZip { get; set; }
        public Guid ScheduleZipId { get; set; }

        private List<Order> _orders;
        public IEnumerable<Order> Orders => _orders.AsReadOnly();

        private List<Plan> _plans;
        public IEnumerable<Plan> Plans => _plans.AsReadOnly();


        private Optimization()
        {
            Id = Guid.NewGuid();
            _orders = new List<Order>();
            _plans = new List<Plan>();           
        }

        public Optimization(int shiftNumber, int shiftLength) : this()
        {
            ShiftNumber = shiftNumber;
            ShiftLength = shiftLength;
            CreationTime = Clock.Now;
        }

        public void AddOrder(Order order)
        {
            Ensure.NotNull(order);
            _orders.Add(order);
        }

        public void AddPlan(Plan plan)
        {
            Ensure.NotNull(plan);
            _plans.Add(plan);
        }
    }
}
