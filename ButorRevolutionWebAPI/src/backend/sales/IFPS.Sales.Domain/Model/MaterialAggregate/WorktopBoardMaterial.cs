namespace IFPS.Sales.Domain.Model
{
    public class WorktopBoardMaterial : BoardMaterial
    {
        public WorktopBoardMaterial()
        {

        }
        public WorktopBoardMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {

        }
    }
}
