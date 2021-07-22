namespace IFPS.Factory.Domain.Model
{
    public class DecorBoardMaterial : BoardMaterial
    {
        private DecorBoardMaterial()
        {

        }
        public DecorBoardMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {

        }
    }
}
