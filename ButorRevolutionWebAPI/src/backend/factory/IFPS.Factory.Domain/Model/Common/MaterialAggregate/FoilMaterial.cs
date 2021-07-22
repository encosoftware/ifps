using IFPS.Factory.Domain.Constants;

namespace IFPS.Factory.Domain.Model
{
    public class FoilMaterial : Material
    {
        /// <summary>
        /// Width of the edging foil
        /// </summary>
        public double Thickness { get; set; }

        private FoilMaterial()
        {

        }

        public FoilMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {
            SiUnitId = SiUnitConstants.MeterSiUnitId;
        }
    }
}
