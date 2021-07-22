namespace IFPS.Factory.Domain.Model
{
    public class Line : Command
    {
        private Line()
        {

        }

        public Line(int succession_number, AbsolutePoint pt) : base(succession_number, pt)
        {
            
        }
    }
}