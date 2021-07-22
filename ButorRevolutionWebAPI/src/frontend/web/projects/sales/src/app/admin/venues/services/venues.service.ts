import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  IVenueViewModel,
  IVenueListViewModel,
  IVenueFilterListViewModel,
  INewVenueViewModel,
  IVenueCountriesListViewModel,
  IVenueDayTypeViewModel,
  ILanguageListViewModel
} from '../models/venues.model';
import { map, take } from 'rxjs/operators';
import { PagedData } from '../../../shared/models/paged-data.model';
import {
  ApiVenuesClient,
  VenueCreateDto,
  VenueDetailsDto,
  PagedListDtoOfVenueListDto,
  VenueUpdateDto,
  AddressUpdateDto,
  AddressCreateDto,
  ApiCountriesClient,
  CountryListDto,
  ApiVenuesDeactivateClient,
  ApiDaytypesClient,
  DayTypeListDto,
  VenueDateRangeUpdateDto,
  ApiLanguagesClient,
  LanguageListDto,
  MeetingRoomUpdateDto,
  CompanyTypeEnum
} from '../../../shared/clients';
import { format } from 'date-fns';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../core/store/selectors/core.selector';
import { CompanyService } from '../../companies/services/company.service';

@Injectable({
  providedIn: 'root'
})
export class VenuesService {

  userCompany: string;

  constructor(
    private venuesClient: ApiVenuesClient,
    private countryClient: ApiCountriesClient,
    private venueActivateClient: ApiVenuesDeactivateClient,
    private dateClient: ApiDaytypesClient,
    private languagesClient: ApiLanguagesClient,
    private companyService: CompanyService,
    private store: Store<any>
  ) {
    this.store.pipe(
      select(coreLoginT),
      take(1)
    ).subscribe(res => {
      this.userCompany = res.CompanyId;
      if (!this.userCompany) {
        this.companyService.getCompanyList({
            id: undefined,
            name: undefined,
            companyType: CompanyTypeEnum.MyCompany,
            address: undefined,
            email: undefined,
            pager: ({
              pageIndex: 0,
              pageSize: 10,
            })
          }).subscribe(c => this.userCompany = c.items[0].id.toString());
      }
    });
  }

  getLanguages(): Observable<ILanguageListViewModel[]> {
    return this.languagesClient.get().pipe(map(res => res.map(this.languageDtoToViewModel)));
  }

  private languageDtoToViewModel(dto: LanguageListDto): ILanguageListViewModel {
    return {
      languageType: dto.languageType,
      translation: dto.translation
    };
  }

  postVenue(venue: INewVenueViewModel): Observable<number> {
    let dto = new VenueCreateDto({
      companyId: +this.userCompany,
      officeAddress: new AddressCreateDto({
        address: venue.address.address,
        city: venue.address.city,
        countryId: venue.address.country,
        postCode: venue.address.postCode
      }),
      name: venue.name,
      email: venue.email,
      phoneNumber: venue.phone
    });
    dto.officeAddress = new AddressCreateDto({
      countryId: venue.address.country,
      postCode: venue.address.postCode,
      city: venue.address.city,
      address: venue.address.address,
    });
    return this.venuesClient.createVenue(dto);
  }

  getVenue(id: number): Observable<IVenueViewModel> {
    return this.venuesClient.details(id).pipe(map(this.venueDetailsToViewModel));
  }

  private venueDetailsToViewModel(dto: VenueDetailsDto): IVenueViewModel {
    let venue: IVenueViewModel = {
      isActive: dto.isActive,
      basicInfo: {
        name: dto.name,
        email: dto.email,
        phone: dto.phoneNumber,
        address: {
          country: dto.officeAddress.countryId,
          postCode: dto.officeAddress.postCode,
          city: dto.officeAddress.city,
          address: dto.officeAddress.address
        },
        openingHours: []
      },
      meetingRooms: []
    };
    for (let meetingRoom of dto.meetingRooms) {
      venue.meetingRooms.push({
        id: meetingRoom.id,
        name: meetingRoom.name,
        location: meetingRoom.description
      });
    }
    for (let openingHours of dto.openingHours) {
      venue.basicInfo.openingHours.push({
        dayType: openingHours.dayType.dayType,
        from: format(openingHours.from, 'HH:mm'),
        to: format(openingHours.to, 'HH:mm'),
        id: openingHours.dayType.id
      });
    }
    return venue;
  }

  putVenue(id: number, venue: IVenueViewModel): Observable<void> {
    let dto = new VenueUpdateDto(
      {
        email: venue.basicInfo.email,
        name: venue.basicInfo.name,
        phoneNumber: venue.basicInfo.phone,
        openingHours: [],
        meetingRooms: []
      }
    );
    dto.officeAddress = new AddressUpdateDto({
      countryId: venue.basicInfo.address.country,
      postCode: venue.basicInfo.address.postCode,
      city: venue.basicInfo.address.city,
      address: venue.basicInfo.address.address
    });
    for (let openingHour of venue.basicInfo.openingHours) {
      const fromArray = openingHour.from.split(':');
      const toArray = openingHour.to.split(':');
      let openingHourDto = new VenueDateRangeUpdateDto({
        dayTypeId: openingHour.id,
        from: new Date(Date.UTC(2019, 1, 1, +fromArray[0], +fromArray[1])),
        to: new Date(Date.UTC(2019, 1, 1, +toArray[0], +toArray[1]))
      });
      dto.openingHours.push(openingHourDto);
    }
    for (let meetingRoom of venue.meetingRooms) {
      let meetingRoomDto = new MeetingRoomUpdateDto({
        id: meetingRoom.id,
        name: meetingRoom.name,
        description: meetingRoom.location
      });
      dto.meetingRooms.push(meetingRoomDto);
    }
    return this.venuesClient.updateVenue(id, dto);
  }

  deleteVenue(id: number): Observable<void> {
    return this.venuesClient.deleteVenue(id);
  }

  deactivateVenue(id: number): Observable<void> {
    return this.venueActivateClient.activateOrDeactivateVenue(id);
  }

  getVenues = (filter: IVenueFilterListViewModel): Observable<PagedData<IVenueListViewModel>> =>
    this.venuesClient.list(filter.name, filter.rooms || undefined, filter.address, filter.phone, filter.email, true, undefined,
      filter.pager.pageIndex, filter.pager.pageSize).pipe(
        map((dto: PagedListDtoOfVenueListDto): PagedData<IVenueListViewModel> => ({
          items: dto.data.map(x => ({
            name: x.name,
            id: x.id,
            rooms: x.rooms,
            address: x.address.postCode + ' ' + x.address.city + ', ' + x.address.address,
            phone: x.phoneNumber,
            email: x.email,
            active: x.isAktiv
          })),
          pageIndex: filter.pager.pageIndex,
          pageSize: filter.pager.pageSize,
          totalCount: dto.totalCount
        }))
      )

  getCountries(): Observable<IVenueCountriesListViewModel[]> {
    return this.countryClient.getCountries().pipe(map(x => x.map(this.countryToViewModel)));
  }

  private countryToViewModel(dto: CountryListDto): IVenueCountriesListViewModel {
    let country: IVenueCountriesListViewModel = {
      id: dto.id,
      code: dto.code,
      translation: dto.translation
    };
    return country;
  }

  getDayTypes(): Observable<IVenueDayTypeViewModel[]> {
    return this.dateClient.getDayTypes().pipe(map(x => x.map(this.daytypeToViewModel)));
  }

  private daytypeToViewModel(dto: DayTypeListDto): IVenueDayTypeViewModel {
    let day: IVenueDayTypeViewModel = {
      dayType: dto.dayType,
      id: dto.id,
      translation: dto.translation
    };
    return day;
  }

}
