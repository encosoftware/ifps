using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class DrillSeed : IEntitySeed<Drill> 
    {
        public Drill[] Entities => new[]
        {
            new Drill(0) { Id = 2, FurnitureComponentId = new Guid("bca55c43-55e1-4c7c-b080-60a12a6af765") },
            new Drill(0) { Id = 3, FurnitureComponentId = new Guid("efbb21c9-39de-417e-8de4-3453d8fc3c1c") },
            new Drill(0) { Id = 4, FurnitureComponentId = new Guid("efbb21c9-39de-417e-8de4-3453d8fc3c1c") },
            new Drill(0) { Id = 5, FurnitureComponentId = new Guid("cd44769e-7d10-4b72-8513-fcec1bba447a") },
            new Drill(0) { Id = 6, FurnitureComponentId = new Guid("cd44769e-7d10-4b72-8513-fcec1bba447a") },
            new Drill(0) { Id = 7, FurnitureComponentId = new Guid("8f092adb-cf14-4d7f-999c-62c68a1d9e87") },
            new Drill(0) { Id = 8, FurnitureComponentId = new Guid("8f092adb-cf14-4d7f-999c-62c68a1d9e87") }
        };

        //public Drill[] Entities => new Drill[] { };

    }
}
