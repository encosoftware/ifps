using System.Linq;
using System.Collections.Generic;

using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Enums;

namespace IFPS.Factory.Domain.Helper
{
    public class CNCCodeConverter
    {
        public static List<List<string>> ConvertUnit(ConcreteFurnitureUnit unit)
        {
            List<List<string>> CncPrograms = new List<List<string>>();
            int i = 0;
            foreach (var concreteComponent in unit.ConcreteFurnitureComponents)
            {
                CncPrograms.Add(ConvertComponent(concreteComponent.FurnitureComponent, i++));
            }
            return CncPrograms;
        }

        public static List<string> ConvertComponent(FurnitureComponent component, int count = 0)
        {
            double x = component.Length;
            double y = component.Width;
            double z = 18.0;

            var actPlane = PlaneTypeEnum.XY;
            var actTool = 3.0;

            List<string> cncProgram = new List<string> { "%", $"O{count} {component.Name}" };

            SaftyBlock(ref cncProgram);

            if (!component.Type.Equals(FurnitureComponentTypeEnum.Front))
            {
                LoadTool(ref cncProgram, actTool);
                FormatingCode(ref cncProgram, x, y, z);
            }

            foreach (var sequence in component.Sequences)
            {
                if (sequence is Rectangle)
                {
                    LoadTool(ref cncProgram, actTool);
                    MillingRectangle(ref cncProgram, (Rectangle)sequence);
                }
                else if (sequence is Drill)
                {
                    var drill = (Drill)sequence;
                    if (!drill.Plane.Equals(actPlane))
                    {
                        ReturnToZero(ref cncProgram);
                        actPlane = drill.Plane;
                        ChangePlane(ref cncProgram, actPlane);
                    }

                    if (drill.Diameter != actTool)
                    {
                        actTool = drill.Diameter;
                        ChangeTool(ref cncProgram, actTool);
                    }
                    Drilling(ref cncProgram, drill);
                }
            }
            EndProgram(ref cncProgram);
            cncProgram.Add("%");
            return cncProgram;
        }

        public static void FormatingCode(ref List<string> cncProgram, double x, double y, double z = 18.0)
        {
            var line = MakeCodeLine("G00", new Dictionary<string, double> { { "X", -3 }, { "Y", y / 2 - 18 }, { "Z", 0 } }, info: "Begin Formating");
            cncProgram.Add(line);

            line = MakeCodeLine("G03", new Dictionary<string, double> { { "X", 0 }, { "Y", y / 2 - 3 }, { "Z", -z - 1 }, { "R", 50 } });
            cncProgram.Add(line);

            foreach (var (pX, pY) in new[] { (0, y), (x, y), (x, 0), (0, 0), (0, y / 2 + 3) })
            {
                line = MakeCodeLine("G01", new Dictionary<string, double> { { "X", pX }, { "Y", pY } });
                cncProgram.Add(line);
            }

            line = MakeCodeLine("G03", new Dictionary<string, double> { { "X", -3 }, { "Y", y / 2 + 18 }, { "Z", 0 }, { "R", 50 } }, info: "End Formating");
            cncProgram.Add(line);
        }

        public static void MillingRectangle(ref List<string> cncProgram, Rectangle rectangle)
        {

            var tl = rectangle.TopLeft;

            var line = MakeCodeLine("G00", new Dictionary<string, double> { { "X", tl.X }, { "Y", tl.Y }, { "Z", 0 } }, info: "Begin Rectangle");
            cncProgram.Add(line);

            line = MakeCodeLine("G01", new Dictionary<string, double> { { "Z", tl.Z } });
            cncProgram.Add(line);

            line = MakeCodeLine("G01", new Dictionary<string, double> { { "X", rectangle.TopRight.X }, { "Y", rectangle.TopRight.Y }, { "Z", rectangle.TopRight.Z } });
            cncProgram.Add(line);

            line = MakeCodeLine("G01", new Dictionary<string, double> { { "X", rectangle.BottomRight.X }, { "Y", rectangle.BottomRight.Y }, { "Z", rectangle.BottomRight.Z } });
            cncProgram.Add(line);

            line = MakeCodeLine("G01", new Dictionary<string, double> { { "X", rectangle.BottomLeft.X }, { "Y", rectangle.BottomLeft.Y }, { "Z", rectangle.BottomLeft.Z } });
            cncProgram.Add(line);

            line = MakeCodeLine("G01", new Dictionary<string, double> { { "X", rectangle.TopLeft.X }, { "Y", rectangle.TopLeft.Y }, { "Z", rectangle.TopLeft.Z } });
            cncProgram.Add(line);

            line = MakeCodeLine("G01", new Dictionary<string, double> { { "Z", 0 } }, info: "End Rectangle");
            cncProgram.Add(line);
        }

        public static void Drilling(ref List<string> cncProgram, Drill drill)
        {
            var h = drill.Holes.First();
            var line = MakeCodeLine("G00", new Dictionary<string, double> { { "X", h.Point.X }, { "Y", h.Point.Y } }, info: "Begin Drilling cicle.");
            cncProgram.Add(line);
            line = MakeCodeLine("G99 G81", new Dictionary<string, double> { { "Z", h.Point.Z }, { "R", 0.5 } });
            cncProgram.Add(line);

            foreach (var hole in drill.Holes.Skip(1))
            {
                line = MakeCodeLine("", new Dictionary<string, double> { { "X", hole.Point.X }, { "Y", hole.Point.Y }, { "Z", hole.Point.Z } });
                cncProgram.Add(line);
            }

            line = MakeCodeLine("G80", info: "End Drilling cicle.");
            cncProgram.Add(line);
        }

        public static void ChangeTool(ref List<string> cncProgram, double diameter)
        {
            TurnOffTool(ref cncProgram, "Begin Tool change");
            LoadTool(ref cncProgram, diameter, "End Tool change");
        }

        public static void TurnOffTool(ref List<string> cncProgram, string info = "")
        {
            cncProgram.Add(MakeCodeLine("M05", info: info));
        }

        public static void LoadTool(ref List<string> cncProgram, double diameter, string info = "")
        {
            cncProgram.Add(MakeCodeLine("M06", new Dictionary<string, double> { { "T", diameter } }));
            cncProgram.Add(MakeCodeLine("M03", info: info));
        }

        public static void ChangePlane(ref List<string> cncProgram, PlaneTypeEnum plane)
        {
            int planeGNumber = 17;
            int mirrorCode = -1;
            if (plane.Equals(PlaneTypeEnum.XZ))
            {
                planeGNumber = 18;
            }
            else if (plane.Equals(PlaneTypeEnum.Mirror_XZ))
            {
                planeGNumber = 18;
                mirrorCode = 21;
            }
            else if (plane.Equals(PlaneTypeEnum.YZ))
            {
                planeGNumber = 19;
            }
            else if (plane.Equals(PlaneTypeEnum.Mirror_YZ))
            {
                planeGNumber = 19;
                mirrorCode = 22;
            }
            var mirrorStr = (mirrorCode == -1) ? "" : $" M{mirrorCode}";
            var planeCode = $"G{planeGNumber}{mirrorStr}";
            cncProgram.Add(MakeCodeLine(planeCode, info: "Change plane"));
        }

        public static void SaftyBlock(ref List<string> cncProgram)
        {

            var line = MakeCodeLine("G17", info: "Begin Safty Block");
            cncProgram.Add(line);

            foreach (var code in new[] { "G21", "G40", "G49" })
            {
                line = MakeCodeLine(code);
                cncProgram.Add(line);
            }

            line = MakeCodeLine("G90", info: "End Safty Block");
            cncProgram.Add(line);
        }

        public static void ReturnToZero(ref List<string> cncProgram)
        {
            cncProgram.Add(MakeCodeLine("G28", new Dictionary<string, double> { { "X", 0 }, { "Y", 0 }, { "Z", 0 } }));
        }

        public static void EndProgram(ref List<string> cncProgram)
        {
            cncProgram.Add(MakeCodeLine("G90", info: "Begin Program End"));
            ReturnToZero(ref cncProgram);
            TurnOffTool(ref cncProgram);
            cncProgram.Add(MakeCodeLine("M30", info: "End Program End"));
        }

        public static string MakeCodeLine(string code, Dictionary<string, double> param = null, string info = "")
        {
            var paramString = "";
            if (param != null)
            {
                foreach (string key in param.Keys)
                {
                    paramString += $" {key}{param[key]}";
                }
            }

            if (info.Length == 0)
            {
                return $"{code}{paramString}";
            }
            return $"{code}{paramString} ;({info})";
        }
    }
}