using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Command : Entity
    {
        /// <summary>
        /// The order of the command execution.
        /// </summary>
        public int SuccessionNumber { get; set; }

        /// <summary>
        /// The command targeted point.
        /// </summary>
        public AbsolutePoint Point { get; set; }
        public int? PointId { get; set; }

        /// <summary>
        /// A sequence which the command is a member.
        /// </summary>
        public virtual Sequence Sequence { get; set; }
        public int? SequenceId { get; set; }

        protected Command()
        {

        }

        public Command(int successionnumber, AbsolutePoint pt) : this()
        {
            SuccessionNumber = successionnumber;
            Point = pt;
        }

        public Command(int successionNumber) : this()
        {
            SuccessionNumber = successionNumber;
        }

        public virtual Tuple<double, double> GetArcParam() { return new Tuple<double, double>(0, 0); }
    }
}