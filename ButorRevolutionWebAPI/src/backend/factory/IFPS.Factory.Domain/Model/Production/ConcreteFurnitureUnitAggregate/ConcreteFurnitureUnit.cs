using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class ConcreteFurnitureUnit : AggregateRoot
    {
        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }

        public virtual FurnitureUnit FurnitureUnit { get; set; }
        public Guid FurnitureUnitId { get; set; }

        public CfuStatusEnum Status { get; set; }

        private List<ManualLaborPlan> _plans;
        public IEnumerable<ManualLaborPlan> Plans => _plans.AsReadOnly();

        private List<ConcreteFurnitureComponent> _concreteFurnitureComponents;
        public IEnumerable<ConcreteFurnitureComponent> ConcreteFurnitureComponents => _concreteFurnitureComponents.AsReadOnly();

        private ConcreteFurnitureUnit()
        {
            _plans = new List<ManualLaborPlan>();
            _concreteFurnitureComponents = new List<ConcreteFurnitureComponent>();
        }

        public ConcreteFurnitureUnit(Order order) : this()
        {
            Order = order;
        }

        public ConcreteFurnitureUnit(Guid orderId) : this()
        {
            OrderId = orderId;
        }

        public void AddCFC(ConcreteFurnitureComponent cfc)
        {
            _concreteFurnitureComponents.Add(cfc);
        }
    }
}
