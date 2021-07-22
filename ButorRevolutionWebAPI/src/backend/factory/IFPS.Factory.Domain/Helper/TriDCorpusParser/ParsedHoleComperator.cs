using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IFPS.Factory.Domain.Helper
{
    public class ParsedHoleComperator : IComparer<ParsedHole>
    {
        public int Compare( ParsedHole x, ParsedHole y)
        {
            if (x.Plane == y.Plane && x.Diameter == y.Diameter)
            {
                return 0;
            }
            else if (x.Plane < y.Plane || (x.Plane == y.Plane && x.Diameter < y.Diameter))
            {
                return -1;
            }
            return 1;
        }
    }
}
