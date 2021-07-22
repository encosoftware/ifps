using IFPS.Factory.Domain.Constants;

namespace IFPS.Factory.Domain.Model
{
    public class BoardMaterial : Material
    {
        public Dimension Dimension { get; set; }
        public bool HasFiberDirection { get; set; }

        protected BoardMaterial()
        {

        }
        public BoardMaterial(string code, int transactionMultiplier = 10) : base(code,transactionMultiplier)
        {
            // every BoardMaterial is stored in board piece number by default
            SiUnitId = SiUnitConstants.PieceSiUnitId;
        }
    }
}
