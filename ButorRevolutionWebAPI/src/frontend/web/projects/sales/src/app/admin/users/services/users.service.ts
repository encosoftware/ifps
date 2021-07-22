import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import {
  IUsersFilterViewModel,
  IUsersListViewModel,
  IUsersCreateListViewModel,
  ICalendarViewModel,
  AbsenceDayViewModel,
  IModuleViewModel,
  IClaimViewModel,
  IUserEditViewModel,
  IAddressViewModel,
  IUserRole,
  IUserNotificationModel,
  UserSalesModel,
  CompanySelectModel,
  RolesSelectModel,
  UsersOfficesModel,
  DayTypeListModel,
  CountryListSelectModel,
  ILanguageSelectList
} from '../models/users.models';
import { map } from 'rxjs/operators';
import { PagedData } from '../../../shared/models/paged-data.model';
import {
  ApiUsersClient,
  UserCreateDto,
  UserListDto,
  PagedListDtoOfUserListDto,
  ApiUsersDeactivateClient,
  ApiUsersAbsencedaysClient,
  AbsenceDayDto,
  AbsenceTypeEnum,
  ClaimGroupDto,
  AbsenceDaysDeleteDto,
  ApiUsersActivateClient,
  UserDetailsDto,
  ApiRolesClient,
  UserDto,
  VenueDto,
  ApiUsersSearchClient,
  ApiCompaniesClient,
  PagedListDtoOfCompanyListDto,
  CompanyTypeEnum,
  RoleListDto,
  UserUpdateDto,
  UpdateWorkingInfoDto,
  WorkingHourDto,
  NotificationsDto,
  ApiDaytypesWeekdaysClient,
  AddressDetailsDto,
  ApiCountriesClient,
  ImageUpdateDto,
  ApiVenuesSearchClient,
  ApiLanguagesClient,
  DivisionTypeEnum,
  ApiClaimsGroupsClient,
  ClaimPolicyEnum,
  ApiUsersEmailClient,
  ApiRolesDivisionListClient,
  ApiNotificationsEventsClient,
} from '../../../shared/clients';
import { format, parse } from 'date-fns';

// tslint:disable:max-line-length


@Injectable()
export class UsersService {
  constructor(
    private usersClient: ApiUsersClient,
    private userDeactivate: ApiUsersDeactivateClient,
    private userAbsenceDay: ApiUsersAbsencedaysClient,
    private claimsClient: ApiClaimsGroupsClient,
    private userActivate: ApiUsersActivateClient,
    private rolesClient: ApiRolesClient,
    private notificationEvent: ApiNotificationsEventsClient,
    private salesPerson: ApiUsersSearchClient,
    private companyClient: ApiCompaniesClient,
    private userOfficesSearch: ApiVenuesSearchClient,
    private dayTypes: ApiDaytypesWeekdaysClient,
    private countries: ApiCountriesClient,
    private language: ApiLanguagesClient,
    private emailValid: ApiUsersEmailClient,
    private rolesDiv: ApiRolesDivisionListClient,
  ) { }

  getUsers(filter: IUsersFilterViewModel): Observable<PagedData<IUsersListViewModel>> {
    const tempFrom = filter.addedOnTo ? new Date(filter.addedOnTo) : undefined;
    const tempTo = filter.addedOnTo ? new Date() : undefined;
    return this.usersClient.get(filter.name, filter.role, filter.status, filter.phone, filter.email, filter.company, tempFrom, tempTo, undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
      map((resp: PagedListDtoOfUserListDto) => ({
        totalCount: resp.totalCount,
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        items: resp.data.map((ins: UserListDto) => ({
          id: ins.id,
          name: ins.name,
          role: ins.role,
          status: ins.isActive,
          company: ins.company,
          email: ins.email,
          image: ins.image,
          phone: ins.phone,
          addedOn: ins.addedOn
        }))
      })),
    );
  }

  getClaimsList(): Observable<IModuleViewModel[]> {
    return this.claimsClient.getClaimsWithGroupsList().pipe(map(this.claimsToViewModel));
  }
  private claimsToViewModel(dto: ClaimGroupDto[]): IModuleViewModel[] {
    let modules: IModuleViewModel[] = [];
    for (let moduleDto of dto) {
      let module: IModuleViewModel = {
        name: moduleDto.name,
        description: moduleDto.detail,
        division: moduleDto.division,
        claims: [],
      };
      for (let claimDto of moduleDto.claims) {
        let claim: IClaimViewModel = {
          id: claimDto.id,
          name: ClaimPolicyEnum[ClaimPolicyEnum[claimDto.name]],
          isChecked: false
        };
        module.claims.push(claim);
      }
      modules.push(module);
    }
    return modules;
  }

  newUser(user: IUsersCreateListViewModel): Observable<number> {
    const userDto = new UserCreateDto({
      name: user.name,
      phoneNumber: user.phoneNumber,
      email: user.email,
      roleId: user.roleId,
      gaveEmailConsent: false
    });
    return this.usersClient.post(userDto).pipe();
  }

  getUserEdit(id: number): Observable<IUserEditViewModel> {
    return this.usersClient.getById(id).pipe(
      map((resp: UserDetailsDto) => ({
        basicInfo: {
          roles: resp.ownedRolesIds,
          name: resp.name,
          email: resp.email,
          language: resp.language,
          company: resp.company ? resp.company.id : null,
          companyList: resp.company,
          address: this.addressDtoToView(resp.address),
          phone: resp.phoneNumber,
          image: resp.image,
          isCompanyRequired: resp.isCompanyNeeded
        },
        isEmployee: resp.isEmployee,
        isActivated: resp.isActive,
        workingInfo: (resp.workingInfo) ? {
          usersOffices: (resp.workingInfo.offices.length) ? resp.workingInfo.offices.map((office: VenueDto): number => office.id) : [],
          usersOfficesList: (resp.workingInfo.offices.length) ? resp.workingInfo.offices.map((office: VenueDto): UsersOfficesModel => ({
            id: office.id,
            name: office.name
          })) : null,
          supervisors: (resp.workingInfo.supervisor) ? resp.workingInfo.supervisor.id : null,
          supervisorsList: (resp.workingInfo.supervisor) ? {
            id: resp.workingInfo.supervisor.id,
            name: resp.workingInfo.supervisor.name,
          } : null,
          supervisees: (resp.workingInfo.supervisees) ? resp.workingInfo.supervisees.map((supervisees: UserDto): IUserRole => ({
            id: supervisees.id,
            name: supervisees.name,
          })) : null,
          groups: (resp.workingInfo.teams) ? resp.workingInfo.teams.map((teams: string): string => teams) : null,
          discount: { from: resp.workingInfo.minDiscountPercent, to: resp.workingInfo.maxDiscountPercent },
          workingHours: resp.workingInfo.workingHours.map((wh) => ({
            dayTypeId: wh.dayTypeId,
            id: wh.id,
            from: wh.from,
            to: wh.to,
          }))
        } : undefined,
        notifications: (resp.notifications) ? {
          notificationType: resp.notifications.notificationTypeFlags,
          eventTypeIds: resp.notifications.eventTypeIds,
        } : undefined,
        claims: (resp.ownedClaimsIds) ? resp.ownedClaimsIds.map((claim) => claim) : undefined,
      })
      ),
    );
  }

  addressDtoToView(resp: IAddressViewModel) {
    if (!resp) {
      return {};
    } else {
      return ({
        address: resp.address,
        postCode: resp.postCode,
        city: resp.city,
        countryId: resp.countryId,
      });
    }
  }

  deleteUsers(id: number): Observable<void> {
    return this.usersClient.deleteUser(id);
  }

  searchSalesPerson(name: string, take = 10): Observable<UserSalesModel[]> {
    return this.salesPerson.searchUser(name, DivisionTypeEnum.Sales, take);
  }

  searchUsersOffices(name: string | null, companyId: number | null | undefined, take = 10): Observable<UsersOfficesModel[]> {
    return this.userOfficesSearch.search(name, companyId, take).pipe(
      map((dto) => dto.map(c => ({ id: c.id, name: c.name })))
    );
  }

  getCompanies(): Observable<CompanySelectModel[]> {
    return this.companyClient.get('', CompanyTypeEnum.None, undefined, undefined, undefined, undefined, undefined).pipe(
      map((dto: PagedListDtoOfCompanyListDto): CompanySelectModel[] => {
        const retObj: CompanySelectModel[] = [];
        dto.data.forEach(x => {
          const tempCompany: CompanySelectModel = {
            id: x.id,
            name: x.name
          };
          retObj.push(tempCompany);
        });
        return retObj;
      })
    );
  }

  updateUsers(id: number, user: IUserEditViewModel): Observable<void> {
    let WorkingHoursDto = [];
    if (user.workingInfo) {
      for (let workingHours of user.workingInfo.workingHours) {
        if (workingHours.isChecked) {
          for (let workingHour of workingHours.workingHour) {
            const fromWh = workingHour.from.toString().split(':');
            const toWh = workingHour.to.toString().split(':');
            WorkingHoursDto.push(new WorkingHourDto({
              id: workingHour.id ? workingHour.id : null,
              dayTypeId: workingHour.dayTypeId,
              from: new Date(Date.UTC(2019, 1, 1, +fromWh[0], +fromWh[1])),
              to: new Date(Date.UTC(2019, 1, 1, +toWh[0], +toWh[1])),
            }));
          }
        }
      }
    }
    const workingInfoDto = new UpdateWorkingInfoDto({
      workingHours: WorkingHoursDto ? WorkingHoursDto : null,
      officeIds: user.workingInfo ? user.workingInfo.usersOffices.map((c) => c) : undefined,
      supervisorUserId: user.workingInfo ? user.workingInfo.supervisors : undefined,
      minDiscountPercent: undefined,
      maxDiscountPercent: undefined,
    });
    const notifDto = new NotificationsDto({
      notificationTypeFlags: user.notifications ? user.notifications.notificationType : undefined,
      eventTypeIds: user.notifications ? user.notifications.eventTypeIds : undefined
    });
    const addressDto = new AddressDetailsDto({
      address: user.basicInfo.address.address,
      postCode: user.basicInfo.address.postCode,
      city: user.basicInfo.address.city,
      countryId: user.basicInfo.address.countryId,
    });

    const imageDto = new ImageUpdateDto({
      containerName: user.basicInfo.image.containerName,
      fileName: user.basicInfo.image.fileName
    });

    const dto = new UserUpdateDto({
      name: user.basicInfo.name,
      phoneNumber: user.basicInfo.phone,
      email: user.basicInfo.email,
      address: addressDto,
      companyId: user.basicInfo.company,
      language: user.basicInfo.language,
      ownedRolesIds: user.basicInfo.roles,
      ownedClaimsIds: user.claims,
      workingInfo: workingInfoDto ? workingInfoDto : null,
      notifications: (notifDto.eventTypeIds && notifDto.notificationTypeFlags) ? notifDto : undefined,
      imageUpdateDto: imageDto
    });
    return this.usersClient.updateUser(id, dto);
  }

  getDaytypes(): Observable<DayTypeListModel[]> {
    return this.dayTypes.getWeekDays().pipe(
      map(days => days.map((x) => ({
        id: x.id,
        dayType: x.dayType,
        translation: x.translation,
        order: x.order,
      })))
    );
  }

  getCountries(): Observable<CountryListSelectModel[]> {
    return this.countries.getCountries().pipe(
      map((cy) => cy.map((c) =>
        ({
          id: c.id,
          name: c.translation
        })))
    );
  }

  getNotificationEvent(): Observable<IUserNotificationModel[] | null> {
    return this.notificationEvent.getEvents().pipe(
      map((resp) =>
        resp.map((ins) =>
          ({
            id: ins.id,
            name: ins.name,            
          })))
    );
  }
  getRoles(): Observable<RolesSelectModel[]> {
    return this.rolesClient.getRoles().pipe(
      map((dto: RoleListDto[]): RolesSelectModel[] => {
        return dto.map(c => ({ name: c.name, id: c.id }));
      }));
  }
  getRolesByid(id: number) {

    return this.rolesClient.getRoleById(id);
  }

  putDeactivate(id: number): Observable<void> {
    return this.userDeactivate.deactivateUser(id);
  }

  putAactivate(id: number): Observable<void> {
    return this.userActivate.activateUser(id);
  }

  deleteAbsenceUsersDay(id: number, dates: Date[]): Observable<void> {
    const dto = new AbsenceDaysDeleteDto({
      dates: dates.map((resp) => resp)
    });
    return this.userAbsenceDay.deleteAbsenceDays(id, dto);
  }

  putAbsenceUsersDay(userId: number, absenceDay: ICalendarViewModel): Observable<void> {
    const absenceDays: AbsenceDayViewModel[] = [...absenceDay.daysOff, ...absenceDay.sickLeave];

    const days = absenceDays.map(resp => new AbsenceDayDto({
      date: resp.date,
      absenceType: resp.absenceType,
    }));
    return this.userAbsenceDay.addOrUpdateAbsenceDays(userId, days);
  }

  getDays(userId: number, year: number, month: number): Observable<ICalendarViewModel | null> {
    return this.userAbsenceDay.getAbsenceDays(userId, year, month + 1).pipe(
      map((resp) => {
        let daysOff = resp.filter((filt: AbsenceDayDto) =>
          filt.absenceType.toString() === AbsenceTypeEnum[AbsenceTypeEnum.DayOff]
        );
        let sickLeave = resp.filter((filt: AbsenceDayDto) => filt.absenceType.toString() === AbsenceTypeEnum[AbsenceTypeEnum.SickLeave]);
        const s: AbsenceDayViewModel[] = sickLeave.map((sick) => {
          return ({
            date: sick.date,
            absenceType: sick.absenceType
          });
        });
        const d: AbsenceDayViewModel[] = daysOff.map((days) => {
          return ({
            date: days.date,
            absenceType: days.absenceType
          });
        });
        let calendar: ICalendarViewModel = {
          sickLeave: s,
          daysOff: d
        };
        return calendar;
      })
    );
  }

  languageSelect() {
    return this.language.get().pipe(
      map((resp): ILanguageSelectList[] => resp.map((select) => ({
        id: select.languageType,
        name: select.translation
      }))
      )
    );
  }
  emailValidation(email): Observable<boolean> {
    return this.emailValid.validateEmail(email);
  }

  getDetailedRolesList(roleIds: number[] | null): Observable<string[] | null> {
    return this.rolesDiv.getDetailedRolesList(roleIds).pipe(
      map(res =>
        res.map(ins => DivisionTypeEnum[DivisionTypeEnum[ins]])
      )
    );
  }
}

