using ENCO.DDD.Domain.Model.Entities;
using IFPS.Sales.Domain.Enums;

namespace IFPS.Sales.Domain.Model
{
    public class Claim : Entity
    {
        /// <summary>
        /// Name of the claim. Unique
        /// </summary>
        public ClaimPolicyEnum Name { get; set; }

        /// <summary>
        /// Module entity, which represents the object 
        /// </summary>
        public virtual Division Division { get; set; }
        public int DivisionId { get; set; }

        public ClaimTypeEnum ClaimType { get; set; }

        private Claim() { }

        public Claim(ClaimPolicyEnum name, int divisionId) : this()
        {
            Name = name;
            DivisionId = divisionId;
        }
    }
}
