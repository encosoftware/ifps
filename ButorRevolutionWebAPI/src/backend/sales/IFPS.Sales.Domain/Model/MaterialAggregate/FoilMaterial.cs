namespace IFPS.Sales.Domain.Model
{
    public class FoilMaterial : Material
    {
        /// <summary>
        /// Width of the edging foil
        /// </summary>
        public double Thickness { get; set; }

        public FoilMaterial()
        {

        }

        public FoilMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {

        }
    }
}
