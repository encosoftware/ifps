namespace IFPS.Sales.Domain.Model
{
    public class AccessoryMaterial : Material
    {
        /// <summary>
        /// True, if the accessory is optional for the customer 
        /// </summary>
        public bool IsOptional { get; private set; }

        /// <summary>
        /// True, if the accessory is required for the assembly
        /// </summary>
        public bool IsRequiredForAssembly { get; private set; }
        public AccessoryMaterial()
        {

        }
        public AccessoryMaterial(bool isOptinal, bool isRequiredForAssembly, string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {
            IsOptional = isOptinal;
            IsRequiredForAssembly = isRequiredForAssembly;
        }
    }
}
