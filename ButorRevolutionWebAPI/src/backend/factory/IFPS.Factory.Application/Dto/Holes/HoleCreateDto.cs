using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class HoleCreateDto
    {
        public int SuccessionNumber { get; set; }
        public AbsolutePointCreateDto RelativePoint { get; set; }

        public HoleCreateDto()
        {

        }

        public Hole CreateModelObject(int sequenceId)
        {
            return new Hole(SuccessionNumber, new AbsolutePoint(0, 0, 0))
            {
                SequenceId = sequenceId
            };
        }
    }
}
