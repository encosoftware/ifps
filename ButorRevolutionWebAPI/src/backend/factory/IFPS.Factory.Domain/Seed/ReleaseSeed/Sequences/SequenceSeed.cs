using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class SequenceSeed : IEntitySeed<Sequence>
    {
        public Sequence[] Entities => new[]
        {
            new Sequence(0) { Id = 1, FurnitureComponentId =  new Guid("b17bfd96-adec-4e40-92ec-89f874499204") }
        };

        //public Sequence[] Entities => new Sequence[] { };
    }
}
