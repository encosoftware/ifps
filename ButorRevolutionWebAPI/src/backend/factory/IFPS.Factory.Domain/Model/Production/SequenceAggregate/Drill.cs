using ENCO.DDD;
using IFPS.Factory.Domain.Enums;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class Drill : Sequence
    {
        /// <summary>
        /// The tool diameter which will be used.
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// The plane where the drilling will be executed.
        /// </summary>
        public PlaneTypeEnum Plane { get; set; }

        /// <summary>
        /// The holes off the drilling  circle.
        /// </summary>
        private List<Hole> _holes = new List<Hole>();
        public IEnumerable<Hole> Holes => _holes.AsReadOnly();

        public void AddHole(Hole hole)
        {
            Ensure.NotNull(hole);
            _holes.Add(hole);
        }


        private Drill()
        {
        }

        public Drill(int successionNumber) : base(successionNumber)
        {

        }
    }
}