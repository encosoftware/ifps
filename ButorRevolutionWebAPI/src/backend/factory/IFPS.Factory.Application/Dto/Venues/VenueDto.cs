using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class VenueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static VenueDto FromModel(Venue venue)
        {
            return new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name
            };
        }
    }
}