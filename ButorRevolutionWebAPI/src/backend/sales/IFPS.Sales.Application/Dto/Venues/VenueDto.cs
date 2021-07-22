using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
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

        public VenueDto()
        {

        }

        public VenueDto(Venue venue)
        {
            Id = venue.Id;
            Name = venue.Name;
        }
    }

}
