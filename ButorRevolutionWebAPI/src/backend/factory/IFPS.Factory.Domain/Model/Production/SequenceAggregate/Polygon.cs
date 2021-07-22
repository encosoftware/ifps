using System.Collections.Generic;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Polygon : Sequence
    {
        /// <summary>
        /// Start point of the polygon milling.
        /// </summary>
        public virtual RelativePoint StartPoint { get; set; }
        public int? StartPointId { get; set; }

        /// <summary>
        /// Commands which are members of the polygon.
        /// </summary>
        private List<Command> _commands;
        public IEnumerable<Command> Commands => _commands.AsReadOnly();

        private Polygon()
        {
            _commands = new List<Command>();
        }

        public Polygon(int succesionnumber)
        {
            
        }

        public override double GetDistance(double sizeX, double sizeY, double sizeZ)
        {
            double distance = 0;

            var previousPoint = _commands[_commands.Count - 1].Point;

            foreach (var command in Commands)
            {
                var actualPoint = command.Point;
                var actualDistance = Math.Sqrt(Math.Pow(actualPoint.X - previousPoint.X, 2) + Math.Pow(actualPoint.Y - previousPoint.Y, 2));

                Tuple<double, double> center = command.GetArcParam();

                if (center.Item1 == 0 && center.Item2 == 0)
                {
                    distance += actualDistance;
                }
                else
                {
                    var hypotenuse = Math.Sqrt(Math.Pow(center.Item1, 2) + Math.Pow(center.Item2, 2));
                    var angle = Math.Asin(actualDistance / (hypotenuse * 2)) * 2;
                    distance += hypotenuse * angle;
                }
                previousPoint = actualPoint;
            }

            return distance;
        }
    }
}