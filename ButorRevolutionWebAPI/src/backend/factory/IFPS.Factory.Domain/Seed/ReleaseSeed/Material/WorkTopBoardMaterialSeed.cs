using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkTopBoardMaterialSeed : IEntitySeed<WorktopBoardMaterial>
    {
        public WorktopBoardMaterial[] Entities => new[]
        {
            new WorktopBoardMaterial("WT001045 (seed)") { Id = new Guid("c634ff73-b129-4cc7-822b-8658ef9b882f"), CurrentPriceId = 1, CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), Description = "Csiszolt fenyő munkalap" }
        };

        //public WorktopBoardMaterial[] Entities => new WorktopBoardMaterial[] { };
    }
}
