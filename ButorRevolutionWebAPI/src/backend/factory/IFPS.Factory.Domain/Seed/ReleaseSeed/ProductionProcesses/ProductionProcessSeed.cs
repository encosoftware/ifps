using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ProductionProcessSeed : IEntitySeed<ProductionProcess>
    {
        //public ProductionProcess[] Entities => new[]
        //{
        //    // Cuttings
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),1) { Id = 1},
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),2) { Id = 2},
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),1) { Id = 3},
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),2) { Id = 4},

        //    // CNC
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),3) { Id = 5},
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),4) { Id = 6},

        //    // EdgeBanding
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),5) { Id = 7},
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),6) { Id = 8},

        //    // Assembly
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),7) { Id = 9},
        //    new ProductionProcess(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"),8) { Id = 10}

        //    // OrderScheduling
        //};
        public ProductionProcess[] Entities => new ProductionProcess[] { };
    }
}
