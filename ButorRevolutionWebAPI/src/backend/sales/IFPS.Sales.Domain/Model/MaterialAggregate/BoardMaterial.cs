namespace IFPS.Sales.Domain.Model
{
    public class BoardMaterial : Material
    {
        public Dimension Dimension { get; set; }
        public bool HasFiberDirection { get; set; }

        public BoardMaterial()
        {

        }
        public BoardMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {

        }
    }
}
