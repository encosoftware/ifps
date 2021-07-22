using System;
using System.Linq.Expressions;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Dbos
{
    public class AppointmentListDbo
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Subject { get; set; }
        public string CustomerName { get; set; }
        public Address Address { get; set; }
        public string Notes { get; set; }
        public string CategoryName { get; set; }
        public int? MeetingRoomId { get; set; }
        public Address OfficeAddress { get; set; }

        public static Expression<Func<Appointment, AppointmentListDbo>> Projection
        {
            get
            {
                return x => new AppointmentListDbo
                {
                    Id = x.Id,
                    From = x.ScheduledDateTime.From,
                    Address = x.Address,
                    To = x.ScheduledDateTime.To,
                    Subject = x.Subject,
                    CustomerName = x.Customer.CurrentVersion.Name,
                    Notes = x.Notes,
                    CategoryName = x.Category.Translations.GetCurrentTranslation().GroupingCategoryName, 
                    OfficeAddress = x.MeetingRoom.Venue.OfficeAddress,
                    MeetingRoomId = x.MeetingRoomId
                };
            }
        }
    }
}
