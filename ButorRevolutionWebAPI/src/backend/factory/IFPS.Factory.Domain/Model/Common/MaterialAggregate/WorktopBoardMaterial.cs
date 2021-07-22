namespace IFPS.Factory.Domain.Model
{
    public class WorktopBoardMaterial : BoardMaterial
    {
        private WorktopBoardMaterial()
        {

        }
        public WorktopBoardMaterial(string code, int transactionMultiplier = 10) : base(code, transactionMultiplier)
        {

        }
    }
}
