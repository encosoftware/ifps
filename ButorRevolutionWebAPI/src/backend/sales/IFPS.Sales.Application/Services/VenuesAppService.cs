using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Exceptions;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class VenuesAppService : ApplicationService, IVenuesAppService
    {
        private readonly IVenueRepository repo;
        private readonly ICompanyRepository companyRepository;

        public VenuesAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IVenueRepository repo,
            ICompanyRepository companyRepository
            ) : base(aggregate)
        {
            this.repo = repo;
            this.companyRepository = companyRepository;
        }

        public async Task<List<VenueDto>> SearchVenueAsync(string name, int? companyId, int take)
        {
            Expression<Func<Venue, bool>> filter = (Venue v) => true;
            if (!string.IsNullOrWhiteSpace(name))
            {
                filter = filter.And(v => v.Name.ToLower().Contains(name.ToLower()));
            }
            if (companyId.HasValue)
            {
                filter = filter.And(v => v.CompanyId == companyId.Value);
            }
            
            var users = await repo.GetVenuesAsync(filter, take);

            return users
                .Select(x => VenueDto.FromModel(x))
                .ToList();
        }

        public async Task<int> CreateVenueAsync(VenueCreateDto dto)
        {
            companyRepository.Get(dto.CompanyId);

            var newVenue = dto.CreateVenue();

            await repo.InsertAsync(newVenue);
            await unitOfWork.SaveChangesAsync();

            return newVenue.Id;
        }

        public async Task<PagedListDto<VenueListDto>> GetVenuesListAsync(VenueFilterDto dto)
        {
            Expression<Func<Venue, bool>> filter = (Venue c) => c.Name != null;

            if (!string.IsNullOrEmpty(dto.Name))
            {
                filter = filter.And(x => x.Name.ToLower().Contains(dto.Name.ToLower().Trim()));
            }
            if (dto.Rooms != 0)
            {
                filter = filter.And(w => w.MeetingRooms.Count() == dto.Rooms);
            }
            if (!string.IsNullOrEmpty(dto.OfficeAddress))
            {
                filter = filter.And(entity => entity.OfficeAddress.PostCode.ToString().ToLower().Contains(dto.OfficeAddress.ToLower().Trim())
                    || entity.OfficeAddress.City.ToLower().Contains(dto.OfficeAddress.ToLower().Trim())
                    || entity.OfficeAddress.AddressValue.Contains(dto.OfficeAddress.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(dto.PhoneNUmber))
            {
                filter = filter.And(w => w.PhoneNumber.ToLower().Contains(dto.PhoneNUmber.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(dto.Email))
            {
                filter = filter.And(w => w.Email.ToLower().Contains(dto.Email.ToLower().Trim()));
            }

            var orderingQuery = dto.Orderings.ToOrderingExpression<Venue>(VenueFilterDto.GetOrderingMapping(), nameof(Venue.Id));

            var allVenues = await repo.GetPagedVenuesAsync(filter, orderingQuery, dto.PageIndex, dto.PageSize);
            return allVenues.ToPagedList(VenueListDto.FromEntity);
        }

        public async Task<VenueDetailsDto> GetVenueDetailsAsync(int venueId)
        {
            var venue = await repo.GetVenueAsync(venueId);
            return new VenueDetailsDto(venue);
        }

        public async Task<List<VenueDto>> GetVenueNamesAsync()
        {
            var venues = await repo.GetAllListAsync();
            return venues.Select(ent => new VenueDto(ent)).ToList();
        }

        public async Task<List<MeetingRoomNameListDto>> GetMeetingRoomNamesAsync(int venueId)
        {
            var venue = await repo.GetVenueAsync(venueId);
            return venue.MeetingRooms.Select(ent => new MeetingRoomNameListDto(ent)).ToList();
        }

        public async Task UpdateVenueAsync(int id, VenueUpdateDto dto)
        {
            var venue = await repo.GetVenueAsync(id);

            if (dto != null)
            {
                if (dto.OpeningHours != null)
                {
                    var openingHoursToAdd = dto.OpeningHours
                        .Where(entity => !venue.OpeningHours.Any(ent => entity.DayTypeId == ent.DayTypeId && entity.From == ent.Interval.From && entity.To == ent.Interval.To))
                        .ToList();

                    var openingHoursToDelete = venue.OpeningHours
                        .Where(entity => !dto.OpeningHours.Any(ent => ent.DayTypeId == entity.DayTypeId && ent.From == entity.Interval.From && ent.To == entity.Interval.To))
                        .ToList();

                    venue.RemoveOpeningHours(openingHoursToDelete.Select(entity => entity.Id).ToList());

                    openingHoursToAdd.ForEach(ent => venue.AddOpeningHour(new VenueDateRange(id, ent.DayTypeId, ent.CreateModelObject())));

                }

                if (dto.MeetingRooms != null)
                {
                    var meetingRoomsToAdd = dto.MeetingRooms.Where(entity => !venue.MeetingRooms.Any(ent => ent.Id == entity.Id)).ToList();
                    var meetingRoomsToDelete = venue.MeetingRooms.Where(ent => !dto.MeetingRooms.Any(entity => entity.Id == ent.Id)).ToList();
                    var meetingRoomsToUpdate = venue.MeetingRooms.Where(ent => dto.MeetingRooms.Any(entity => entity.Id == ent.Id)).ToList();

                    foreach (var meetingroom in meetingRoomsToUpdate)
                    {
                        var validationBuilder = new ValidationExceptionBuilder<VenueUpdateDto>();

                        var dtoMeetingRoom = dto.MeetingRooms.Single(ent => ent.Id == meetingroom.Id);

                        if (!meetingroom.Name.Equals(dtoMeetingRoom.Name))
                        {
                            meetingroom.Name = dtoMeetingRoom.Name;
                        }
                        if (!meetingroom.Description.Equals(dtoMeetingRoom.Description))
                        {
                            meetingroom.Description = dtoMeetingRoom.Description;
                        }
                    }

                    meetingRoomsToAdd.ForEach(ent => venue.AddMeetingRoom(new MeetingRoom(ent.Name, ent.Description, venue.Id)));

                    meetingRoomsToDelete.ForEach(ent => 
                    { 
                        ent.IsDeleted = true; 
                        ent.DeletionTime = DateTime.Now; 
                    });

                }

                venue = dto.UpdateModelObject(venue);
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteVenueAsync(int id)
        {
            await repo.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ActivateOrDeactivateVenueAsync(int id)
        {
            var venue = await repo.GetAsync(id);

            if (venue.IsActive)
            {
                venue.IsActive = false;
            }
            else
            {
                venue.IsActive = true;
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<MeetingRoomAppointmentsDto>> GetMeetingRoomsByCompanyAsync(int companyId)
        {
            var venues = await repo.GetVenuesByCompanyAsync(companyId);
            var meetingRooms = new List<MeetingRoomAppointmentsDto>();
            foreach (var venue in venues)
            {
                meetingRooms.AddRange(venue.MeetingRooms.Select(ent => new MeetingRoomAppointmentsDto(ent)).ToList());
            }
            return meetingRooms;
        }
    }
}
