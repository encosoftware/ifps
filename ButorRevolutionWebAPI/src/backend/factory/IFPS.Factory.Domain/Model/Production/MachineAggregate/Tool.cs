using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class Tool : Entity
    {
        /// <summary>
        /// The tool's type is mill or drill tool.
        /// </summary>
        public ToolTypeEnum Type { get; set; }

        /// <summary>
        /// Diameter of the tool. 
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// The machine with own the tool.
        /// </summary>
        public virtual CncMachine CncMachine { get; set; }
        public int CncMachineId { get; set; }
    }
}