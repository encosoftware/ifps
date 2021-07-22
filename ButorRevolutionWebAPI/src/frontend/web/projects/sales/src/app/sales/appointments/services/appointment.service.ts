import { Injectable } from '@angular/core';
import {
    ApiAppointmentsClient,
    ApiAppointmentsRangeClient,
    AppointmentListDto,
    AppointmentDetailsDto,
    AppointmentCreateDto,
    AddressCreateDto,
    AppointmentUpdateDto,
    AddressUpdateDto,
    ApiAppointmentsDatesClient,
    AppointmentDateRangeDto,
    ApiUsersSearchClient,
    DivisionTypeEnum,
    UserDto,
    ApiVenuesNamesClient,
    ApiVenuesMeetingroomsClient,
    VenueDto,
    MeetingRoomNameListDto,
    ApiUsersCompanyClient,
    UserNameDto,
    GroupingCategoryEnum,
    GroupingCategoryListDto,
    ApiCountriesClient,
    CountryListDto,
    ApiOrdersClient,
    PagedListDtoOfOrderListDto,
    ApiOrdersCustomerClient,
    UserDropdownAvatarDto,
    ApiGroupingcategoriesDropdownClient
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import {
    IAppointmentListViewModel,
    IAppointmentDetailViewModel,
    IPersonViewModel,
    IDropDownViewModel,
    IDropDownStringViewModel
} from '../models/appointments.model';
import { map, take } from 'rxjs/operators';
import { format } from 'date-fns';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../core/store/selectors/core.selector';

@Injectable({
    providedIn: 'root'
})
export class AppointmentsService {

    userId: string;

    constructor(
        private appointmentsClient: ApiAppointmentsClient,
        private appointmnetsRangeClient: ApiAppointmentsRangeClient,
        private appointmentDateClient: ApiAppointmentsDatesClient,
        private userSerachClient: ApiUsersSearchClient,
        private venuesClient: ApiVenuesNamesClient,
        private meetingRoomClient: ApiVenuesMeetingroomsClient,
        private userParticipantClient: ApiUsersCompanyClient,
        private groupingCategoryClient: ApiGroupingcategoriesDropdownClient,
        private countryClient: ApiCountriesClient,
        private orderClient: ApiOrdersCustomerClient,
        private store: Store<any>,
        private ordersClient: ApiOrdersClient,
    ) {
        this.store.pipe(
            select(coreLoginT),
            take(1)
        ).subscribe(res => this.userId = res.UserId);
    }

    getAppointments(startDate: Date, endDate: Date): Observable<IAppointmentListViewModel[]> {

        return this.appointmnetsRangeClient.getAppointmentsByDateRange(+this.userId, startDate, endDate)
            .pipe(map(res => res.map(this.appointmentsToViewModel)));
    }

    private appointmentsToViewModel(dto: AppointmentListDto): IAppointmentListViewModel {
        return {
            id: dto.id,
            subject: dto.subject,
            from: dto.from,
            to: dto.to,
            notes: dto.notes,
            customerName: dto.customerName,
            categoryName: dto.categoryName,
            address: {
                postCode: dto.address.postCode,
                address: dto.address.address,
                city: dto.address.city
            }
        };
    }

    deleteAppointment(id: number): Observable<void> {
        return this.appointmentsClient.deleteAppointment(id);
    }

    getAppointment(id: number): Observable<IAppointmentDetailViewModel> {
        return this.appointmentsClient.getAppointmentById(id).pipe(map(this.appointmentToViewModel));
    }

    private appointmentToViewModel(dto: AppointmentDetailsDto): IAppointmentDetailViewModel {
        return {
            subject: dto.subject,
            date: dto.from,
            from: format(dto.from, 'HH:mm'),
            to: format(dto.to, 'HH:mm'),
            notes: dto.notes,
            address: dto.meetingRoomId === 0 ? {
                countryId: dto.address.countryId,
                postCode: dto.address.postCode,
                address: dto.address.address,
                city: dto.address.city
            } : {
                    countryId: undefined,
                    postCode: undefined,
                    address: '',
                    city: ''
                },
            categoryId: dto.categoryId,
            customerId: dto.customerId,
            customerName: dto.customerName,
            isNewAddress: dto.meetingRoomId === 0 ? true : false,
            meetingRoomId: dto.meetingRoomId,
            orderId: dto.orderId,
            venueId: dto.venueId,
            partnerId: dto.partnerId
        };
    }

    postAppointment(model: IAppointmentDetailViewModel): Observable<number> {
        const fromArray = model.from.split(':');
        const toArray = model.to.split(':');
        let dto = new AppointmentCreateDto({
            subject: model.subject,
            from: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +fromArray[0], +fromArray[1])),
            to: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +toArray[0], +toArray[1])),
            notes: model.notes,
            orderId: model.orderId,
            categoryId: model.categoryId,
            customerId: model.customerId,
            partnerId: model.partnerId
        });
        if (model.meetingRoomId) {
            dto.meetingRoomId = model.meetingRoomId;
        } else {
            dto.addressCreateDto = new AddressCreateDto({
                countryId: model.address.countryId,
                postCode: model.address.postCode,
                city: model.address.city,
                address: model.address.address
            });
        }
        return this.appointmentsClient.createAppointment(dto);
    }

    putAppointment(id: number, model: IAppointmentDetailViewModel): Observable<void> {
        const fromArray = model.from.split(':');
        const toArray = model.to.split(':');
        let dto = new AppointmentUpdateDto({
            subject: model.subject,
            from: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +fromArray[0], +fromArray[1])),
            to: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +toArray[0], +toArray[1])),
            notes: model.notes,
            categoryId: model.categoryId,
            customerId: model.customerId,
            partnerId: model.partnerId
        });
        if (model.meetingRoomId > 0) {
            dto.meetingRoomId = model.meetingRoomId;
        } else {
            dto.address = new AddressUpdateDto({
                countryId: model.address.countryId,
                postCode: model.address.postCode,
                city: model.address.city,
                address: model.address.address
            });
        }
        return this.appointmentsClient.updateAppointment(id, dto);
    }

    putDragedAppointment(id: number, from: Date, to: Date): Observable<void> {
        let dto = new AppointmentDateRangeDto({
            from: new Date(Date.UTC(from.getFullYear(), from.getMonth(), from.getDate(), + from.getHours(), + from.getMinutes())),
            to: new Date(Date.UTC(to.getFullYear(), to.getMonth(), to.getDate(), + to.getHours(), + to.getMinutes()))
        });
        return this.appointmentDateClient.updateAppointmentDate(id, dto);
    }

    getContactPersons(name: string, take: number): Observable<IPersonViewModel[]> {
        name = (name === undefined) ? '' : name;
        return this.userSerachClient.searchUser(name, DivisionTypeEnum.Sales, take).pipe(
            map((dto: UserDto[]): IPersonViewModel[] => {
                return dto.map((user: UserDto): IPersonViewModel => {
                    return {
                        id: user.id,
                        name: user.name
                    };
                });
            })
        );
    }

    getSalesPersons(): Observable<IPersonViewModel[]> {
        return this.userParticipantClient.getUserNamesByCompany().pipe(
            map((dto: UserNameDto[]): IPersonViewModel[] => {
                return dto.map((user: UserNameDto): IPersonViewModel => {
                    return {
                        isTechnicalAccount: user.isTechnicalAccount,
                        id: user.id,
                        name: user.name
                    };
                });
            })
        );
    }

    getVenues(): Observable<IDropDownViewModel[]> {
        return this.venuesClient.getVenueNames().pipe(
            map((dto: VenueDto[]): IDropDownViewModel[] => {
                return dto.map((venue: VenueDto): IDropDownViewModel => {
                    return {
                        id: venue.id,
                        name: venue.name
                    };
                });
            })
        );
    }

    getMeetingRooms(id: number): Observable<IDropDownViewModel[]> {
        return this.meetingRoomClient.getMeetingRoomNames(id).pipe(
            map((dto: MeetingRoomNameListDto[]): IDropDownViewModel[] => {
                return dto.map((meetingRoom: MeetingRoomNameListDto): IDropDownViewModel => {
                    return {
                        id: meetingRoom.id,
                        name: meetingRoom.name
                    };
                });
            })
        );
    }

    getAppointmentTypes(): Observable<IDropDownViewModel[]> {
        return this.groupingCategoryClient.getCategoriesForDropdown(GroupingCategoryEnum.AppointmentType, false).pipe(
            map((dto: GroupingCategoryListDto[]): IDropDownViewModel[] => {
                return dto.map((type: GroupingCategoryListDto): IDropDownViewModel => {
                    return {
                        id: type.id,
                        name: type.name
                    };
                });
            })
        );
    }

    getCountries(): Observable<IDropDownViewModel[]> {
        return this.countryClient.getCountries().pipe(
            map((dto: CountryListDto[]): IDropDownViewModel[] => {
                return dto.map((country: CountryListDto): IDropDownViewModel => {
                    return {
                        id: country.id,
                        name: country.translation
                    };
                });
            })
        );
    }

    getOrdersList(): Observable<IDropDownStringViewModel[]> {
        return this.ordersClient.getOrders(undefined, undefined, undefined, undefined, undefined,
            undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined,
            undefined, undefined).pipe(
                map((dto: PagedListDtoOfOrderListDto): IDropDownStringViewModel[] => dto.data.map(res => ({
                    id: res.id,
                    name: res.orderName
                })))
            );
    }

    getCustomerNameByOrderId(orderId: string): Observable<IDropDownViewModel[]> {
        return this.orderClient.getCustomerByOrderId(orderId).pipe(
            map((dto: UserDropdownAvatarDto) => [
                ({
                    id: dto.id,
                    name: dto.name
                })
            ]
            )
        );
    }
}
