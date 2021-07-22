namespace IFPS.Factory.Domain.Model
{
    public class Hole : Command
    {
        public Drill Drill { get; set; }
        public int DrillId { get; set; }

        private Hole()
        {

        }

        public Hole(int succession_number, AbsolutePoint pt) : base(succession_number, pt)
        {

        }

        public Hole(int successionNumber) : base(successionNumber) { }
    }
}