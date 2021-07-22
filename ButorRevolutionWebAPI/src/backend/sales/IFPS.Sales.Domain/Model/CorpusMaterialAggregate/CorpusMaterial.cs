using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class CorpusMaterial : FullAuditedAggregateRoot
    {
        public BoxDimension Dimensions { get; set; }

        /// <summary>
        /// Additional description, provided by the customer
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Outer walls material of the corpus
        /// </summary>
        public virtual BoardMaterial WallMaterial { get; set; }
        public int? WallMaterialId { get; set; }

        /// <summary>
        /// Inner elements material of the corpus (e.g. shelves)
        /// </summary>
        public virtual BoardMaterial InnerMaterial { get; set; }
        public int? InnerMaterialId { get; set; }

        /// <summary>
        /// Back panel material of the corpus 
        /// </summary>
        public virtual BoardMaterial BackPanelMaterial { get; set; }
        public int? BackPanelMaterialId { get; set; }

        /// <summary>
        /// Front material of the corpus
        /// </summary>
        public virtual BoardMaterial FrontMaterial { get; set; }
        public int? FrontMaterialId { get; set; }

        private CorpusMaterial()
        {
        }

        public CorpusMaterial(
            BoxDimension dimensions,
            string description,
            int? wallMaterialId,
            int? innerMaterialId,
            int? backpanelMaterialId,
            int? frontMaterialId
            )
        {
            this.Dimensions = dimensions;            
            this.Description = description;

            this.WallMaterialId = wallMaterialId;
            this.InnerMaterialId = InnerMaterialId;
            this.BackPanelMaterialId = backpanelMaterialId;
            this.FrontMaterialId = frontMaterialId;
        }
    }
}
