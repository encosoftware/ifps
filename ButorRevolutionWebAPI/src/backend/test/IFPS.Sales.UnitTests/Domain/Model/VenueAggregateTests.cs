using FluentAssertions;
using IFPS.Sales.Domain.Model;
using System.Linq;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class VenueAggregateTests
    {
        [Fact]
        public void Empty_venue_has_zero_meetingroom()
        {
            var venue = new Venue("Parlamant", "654949846165", "", 1,
                new Address(1050, "Budapest", "Kossúth tér 2", 1));

            venue.MeetingRooms.Count().Should().Be(0);
        }
    }
}