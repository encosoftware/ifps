using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Helper
{
    public class XXLParser
    {
        private readonly Regex headParser = new Regex(@"H DX=(?<XInt>\d+)\.?(?<XDouble>\d*) DY=(?<YInt>\d+)\.?(?<YDouble>\d*) DZ=(?<ZInt>\d+)\.?(?<ZDouble>\d*).*");
        private readonly Regex longParser = new Regex(@"LONG X=(?<X0Int>\d+)\.?(?<X0Double>\d*) Y=(?<Y0Int>\d+)\.?(?<Y0Double>\d*).+x=(?<X1Int>\d+)\.?(?<X1Double>\d*) y=(?<Y1Int>\d+)\.?(?<Y1Double>\d*) Z=(?<ZInt>\d+)\.?(?<ZDouble>\d*).*");
        private readonly Regex holeParser = new Regex(@"XBO X=(?<XInt>\d+)\.?(?<XDouble>\d*) Y=(?<YInt>\d+)\.?(?<YDouble>\d*) Z=(?<ZInt>\d+)\.?(?<ZDouble>\d*) F(?<Plane>\d).+D=(?<DInt>\d+)\.?(?<DDouble>\d*).*");

        public FurnitureComponent FurnitureComponent { get; set; }

        public XXLParser(FurnitureComponent furnitureComponent)
        {
            this.FurnitureComponent = furnitureComponent;
        }

        public void ParseProgramString(string[] programString)
        {            
            var holes = new List<ParsedHole>();
            var nutes = new List<ParsedNut>();

            Match match = headParser.Match(programString[0]);
            if (!match.Success)
            {
                throw new ArgumentException("The given program string doesn't contain a header!");
            }    
            GroupCollection groups = match.Groups;

            var size = new ParsedSize(groups);

            foreach (string line in programString.Skip(1))
            {
                match = holeParser.Match(line);
                groups = match.Groups;
                if (match.Success)
                {
                    holes.Add(new ParsedHole(groups));

                    continue;
                }

                match = longParser.Match(line);
                groups = match.Groups;

                if (match.Success)
                {
                    nutes.Add(new ParsedNut(groups));
                }
            }
            ConvertParsedProgram(size, nutes, holes);
        }

        private void ConvertParsedProgram(ParsedSize size, List<ParsedNut> nutes, List<ParsedHole> holes)
        {
            int seqNumber = 0;
            MakeRectangles(nutes, ref seqNumber);
            MakeDrills(holes, size, ref seqNumber);
        }
               
        private void MakeRectangles(List<ParsedNut> nutes, ref int seqNumber)
        {
            var selectCorrectCoordinate = new Func<double, double, (double, double, double, double)>(
                (a, b) => (a == b) ? (a - 2, a + 2, a - 2, a + 2) : (a > b ? (a, a, b, b) : (b, b, a, a))
                );
            foreach (var nut in nutes)
            {
                var (xTL, xTR, xBL, xBR) = selectCorrectCoordinate(nut.X0, nut.X1);
                var (yTL, yTR, yBL, yBR) = selectCorrectCoordinate(nut.Y0, nut.Y1);
                var z = -nut.Z;

                seqNumber += 1;

                var rect = new Rectangle(seqNumber);

                rect.TopLeft = new AbsolutePoint(xTL, yTL, z);
                rect.TopRight = new AbsolutePoint(xTR, yTR, z);
                rect.BottomRight = new AbsolutePoint(xBR, yBR, z);
                rect.BottomLeft = new AbsolutePoint(xBL, yBL, z);

                FurnitureComponent.AddSequence(rect);
            }
        }

        private void MakeDrills(List<ParsedHole> holes, ParsedSize size, ref int seqNumber)
        {
            holes.Sort(new ParsedHoleComperator());

            var holePtFunctions = new Dictionary<int, Func<ParsedHole, AbsolutePoint>>()
            {
                { 1, new Func<ParsedHole, AbsolutePoint>(hole => new AbsolutePoint(hole.X, hole.Y, -hole.Z)) },
                { 2, new Func<ParsedHole, AbsolutePoint>(hole => new AbsolutePoint(hole.Z, hole.Y, -hole.X)) },
                { 3, new Func<ParsedHole, AbsolutePoint>(hole => new AbsolutePoint(size.X - hole.Z, hole.Y, -hole.X)) },
                { 4, new Func<ParsedHole, AbsolutePoint>(hole => new AbsolutePoint(hole.X, hole.Z, -hole.Y)) },
                { 5, new Func<ParsedHole, AbsolutePoint>(hole => new AbsolutePoint(hole.X, size.Y - hole.Z, -hole.Y)) },
                { 6, new Func<ParsedHole, AbsolutePoint>(hole => new AbsolutePoint(hole.X, hole.Y, hole.Y - size.Z)) }
            };
                        
            var (prevPlane, prevDiameter) = (0, 0.0);
            int holeNumber = 0;
            Drill drill = null;
            foreach (var hole in holes)
            {
                if (hole.Plane != prevPlane || prevDiameter != hole.Diameter || drill == null)
                {
                    holeNumber = 0;
                    prevDiameter = hole.Diameter;
                    prevPlane = hole.Plane;
                    seqNumber += 1;

                    drill = new Drill(seqNumber);
                    drill.Diameter = prevDiameter;
                    drill.Plane = (Enums.PlaneTypeEnum)prevPlane;

                    FurnitureComponent.AddSequence(drill);
                    Console.WriteLine($"{seqNumber} :: Drill {prevPlane} {prevDiameter}");
                }
                holeNumber += 1;
                var tempHole = new Hole(holeNumber, (holePtFunctions[hole.Plane](hole))); //add hole commands
                drill.AddHole(tempHole);
            }
        }
    }

    public static class XXLParserHelper
    {
        public static double ConvertToDouble(string partInt, string partDouble)
        {
            double retValue;
            string doubleAsString = $"{partInt},{partDouble}";
            if (Double.TryParse(doubleAsString, out retValue))
            {
                return retValue;
            }
            throw new ArgumentException($"{doubleAsString} can not be converted into double!");
        }
    }
}
