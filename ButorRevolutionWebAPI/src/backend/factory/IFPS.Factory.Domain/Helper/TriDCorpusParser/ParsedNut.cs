using System.Text.RegularExpressions;

namespace IFPS.Factory.Domain.Helper
{
    public class ParsedNut
    {
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double Z { get; set; }

        public ParsedNut(GroupCollection groups)
        {
            this.X0 = XXLParserHelper.ConvertToDouble(groups["X0Int"].Value, groups["X0Double"].Value);
            this.X1 = XXLParserHelper.ConvertToDouble(groups["X1Int"].Value, groups["X1Double"].Value);
            this.Y0 = XXLParserHelper.ConvertToDouble(groups["Y0Int"].Value, groups["Y0Double"].Value);
            this.Y1 = XXLParserHelper.ConvertToDouble(groups["Y1Int"].Value, groups["Y1Double"].Value);
            this.Z = XXLParserHelper.ConvertToDouble(groups["ZInt"].Value, groups["ZDouble"].Value);
        }
    }
}
