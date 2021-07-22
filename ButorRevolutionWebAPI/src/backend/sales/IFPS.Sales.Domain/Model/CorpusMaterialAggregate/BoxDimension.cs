using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Sales.Domain.Model
{
    public class BoxDimension : ValueObject<BoxDimension>
    {
        /// <summary>
        /// Height of the corpus, measured in millimeters
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Width of the corpus, measured in millimeters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Depth of the corpus, measured in millimeters
        /// </summary>
        public double Depth { get; set; }
    }
}
