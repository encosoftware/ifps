using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Groove : Entity
    {
        public virtual FurnitureComponent FurnitureComponent { get; set; }
        public Guid FurnitureComponentId { get; set; }

        public virtual Rectangle Rectangle { get; set; }
    }
}
