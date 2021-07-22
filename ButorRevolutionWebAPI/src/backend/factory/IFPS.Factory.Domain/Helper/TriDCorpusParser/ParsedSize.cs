using System.Text.RegularExpressions;

namespace IFPS.Factory.Domain.Helper
{
    public class ParsedSize
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public ParsedSize(GroupCollection groups)
        {
            this.X = XXLParserHelper.ConvertToDouble(groups["XInt"].Value, groups["XDouble"].Value);
            this.Y = XXLParserHelper.ConvertToDouble(groups["YInt"].Value, groups["YDouble"].Value);
            this.Z = XXLParserHelper.ConvertToDouble(groups["ZInt"].Value, groups["ZDouble"].Value);
        }
    }
}
