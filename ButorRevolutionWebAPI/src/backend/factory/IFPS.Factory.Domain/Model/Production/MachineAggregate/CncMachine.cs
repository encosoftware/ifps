using IFPS.Factory.Domain.Enums;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class CncMachine : Machine
    {
        /// <summary>
        /// Coolant type of the machine.
        /// </summary>
        public CoolantTypeEnum CoolantType { get; set; }

        /// <summary>
        /// Return height for the machine.
        /// </summary>
        public double ReturnHeight { get; set; }

        /// <summary>
        /// List of planes on which the machine can not working.
        /// </summary>
        private List<PlaneType> _unavailablePlanes;
        public IEnumerable<PlaneType> UnavailablePlanes => _unavailablePlanes.AsReadOnly();

        /// <summary>
        /// Estimation values as drill-, milling-, rapid speed and tool-, plane- change time.
        /// </summary>
        public EstimatorProperties EstimatorProperties { get; set; }

        /// <summary>
        /// Necessary milling data for code generation.
        /// </summary>
        public MillingProperties MillingProperties { get; set; }

        /// <summary>
        /// Necessary drill data for code generation.
        /// </summary>
        public DrillProperties DrillPropeties { get; set; }

        /// <summary>
        /// Path to the machine's shared folder where the CNC code files should be uploaded
        /// </summary>
        public string SharedFolderPath { get; set; }

        /// <summary>
        /// The available tools.
        /// </summary>
        private List<Tool> _tools;
        public IEnumerable<Tool> Tools => _tools.AsReadOnly();

        public CncMachine(string name, int? brandId) : base(name, brandId)
        {
            _unavailablePlanes = new List<PlaneType>();
            _tools = new List<Tool>();
        }
    }
}
