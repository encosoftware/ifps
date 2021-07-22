using IFPS.Factory.Domain.Constants;

namespace IFPS.Factory.Domain.Model
{
    public class ApplianceMaterial : Material
    {
        /// <summary>
        /// Company, who produced the appliance
        /// </summary>
        public virtual Company Brand { get; set; }
        public int? BrandId { get; set; }

        /// <summary>
        /// Distributor, who was the rights to sail/resail this appliance
        /// </summary>
        public virtual Company Distributor { get; set; }
        public int? DistributorId { get; set; }
        
        /// <summary>
        /// Unique production code of the appliance
        /// </summary>
        public string HanaCode { get; set; }

        /// <summary>
        /// Sell price of the appliance
        /// </summary>
        public Price SellPrice { get; set; }

        private ApplianceMaterial()
        {

        }

        public ApplianceMaterial(string code) : base(code)
        {
            SiUnitId = SiUnitConstants.PieceSiUnitId;
        }
    }
}
