using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class DecorBoardMaterialCreateFromFileDto
    {
        public DecorBoardMaterial CreateModelObject(string code, double width, bool hasFiberDirection)
        {
            return new DecorBoardMaterial(code)
            {
                HasFiberDirection = hasFiberDirection,
                Dimension = new Dimension(width, 0, 0)
            };
        }
    }
}
