using System;
using System.Text.RegularExpressions;

namespace IFPS.Factory.Domain.Helper
{
    public class ParsedHole
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Diameter { get; set; }
        public int Plane { get; set; }

        public ParsedHole(GroupCollection groups)
        {
            this.X = XXLParserHelper.ConvertToDouble(groups["XInt"].Value, groups["XDouble"].Value);
            this.Y = XXLParserHelper.ConvertToDouble(groups["YInt"].Value, groups["YDouble"].Value);
            this.Z = XXLParserHelper.ConvertToDouble(groups["ZInt"].Value, groups["ZDouble"].Value);

            this.Diameter = XXLParserHelper.ConvertToDouble(groups["DInt"].Value, groups["DDouble"].Value);

            int plane;
            if (Int32.TryParse(groups["Plane"].Value, out plane))
            {
                this.Plane = plane;
            }
            else
            {
                this.Plane = 0;
            }
        }
    }
}
