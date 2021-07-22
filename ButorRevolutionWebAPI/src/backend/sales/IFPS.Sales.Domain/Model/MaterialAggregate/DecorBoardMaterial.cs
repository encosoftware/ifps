namespace IFPS.Sales.Domain.Model
{
    public class DecorBoardMaterial : BoardMaterial
    {
        public DecorBoardMaterial()
        {

        }
        public DecorBoardMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {

        }
    }
}
