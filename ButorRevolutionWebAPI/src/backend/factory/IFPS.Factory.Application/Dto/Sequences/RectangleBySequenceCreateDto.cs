using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class RectangleBySequenceCreateDto
    {
        public int SuccessionNumber { get; set; }
        public AbsolutePointCreateDto TopLeft { get; set; }
        public AbsolutePointCreateDto BottomLeft { get; set; }
        public AbsolutePointCreateDto BottomRight { get; set; }
        public AbsolutePointCreateDto TopRight { get; set; }

        public Rectangle CreateModelObject()
        {
            return new Rectangle(SuccessionNumber)
            {
                TopLeft = TopLeft.CreateModelObject(),
                BottomLeft = BottomLeft.CreateModelObject(),
                BottomRight = BottomRight.CreateModelObject(),
                TopRight = TopRight.CreateModelObject()
            };
        }
    }
}
