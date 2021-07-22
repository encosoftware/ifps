import { Injectable } from '@angular/core';
import {
  ICompanyDetailsModel,
  ICompanyListModel,
  ICompanyFilterModel,
  ICompanyCreateModel,
  ICompanyTypeListModel,
  IDayTypesListModel,
  IContactPersonModel,
  ICountriesListModel,
  IOpeningHoursModel
} from '../models/company.model';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiCompaniesClient,
  CompanyCreateDto,
  AddressCreateDto,
  CompanyUpdateDto,
  CompanyDateRangeUpdateDto,
  CompanyTypeListDto,
  DayTypeListDto,
  ApiCompanytypesClient,
  ApiDaytypesClient,
  PagedListDtoOfCompanyListDto,
  ApiCountriesClient,
  CountryListDto,
  ApiUsersSearchClient,
  UserTeamListDto,
  UserDto,
  UserTeamUpdateDto,
  DivisionTypeEnum
} from '../../../shared/clients';
import { IUserTeamModel, IUserListModel } from '../models/group.model';
import { format, getHours, getMinutes, subMinutes, addMinutes } from 'date-fns';
import { IPictureModel } from '../../materials/models/decorboards.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  constructor(
    private companyClient: ApiCompaniesClient,
    private companyTypeClient: ApiCompanytypesClient,
    private dayTypeClient: ApiDaytypesClient,
    private countryClient: ApiCountriesClient,
    private contactClient: ApiUsersSearchClient,
  ) { }

  postCompany(company: ICompanyCreateModel): Observable<number> {
    const dto = new CompanyCreateDto({
      name: company.name,
      address: new AddressCreateDto({
        address: company.address.address,
        city: company.address.city,
        countryId: company.address.countryId,
        postCode: company.address.postCode,
      }),
      companyTypeId: company.companyTypeId,
      registerNumber: company.registerNumber,
      taxNumber: company.taxNumber
    });
    return this.companyClient.createCompany(dto);
  }

  getCompany(id: number): Observable<ICompanyDetailsModel> {
    return this.companyClient.getById(id).pipe(
      map((dto): ICompanyDetailsModel => ({
        id: dto.id,
        name: dto.name,
        companyTypeId: dto.companyType.id,
        taxNumber: dto.taxNumber,
        registerNumber: dto.registerNumber,
        address: {
          address: dto.address.address,
          city: dto.address.city,
          countryId: dto.address.countryId,
          postCode: dto.address.postCode
        },
        contactPerson: {
          id: dto.contactPerson.id,
          email: dto.contactPerson.email,
          name: dto.contactPerson.name,
          phoneNumber: dto.contactPerson.phoneNumber,
        },
        openingHours: dto.openingHours.map((oh): IOpeningHoursModel => ({
          dayTypeId: oh.dayTypeId,
          from: format(oh.from, 'HH:mm'),
          to: format(oh.to, 'HH:mm')
        })),
        userTeams: dto.userTeams.map((ut: UserTeamListDto): IUserTeamModel => ({
          id: ut.id,
          name: ut.name,
          users: ut.users.map((user: UserDto): IUserListModel => {
            let tempPicture: IPictureModel;
            if (!user.image) {
              tempPicture = {
                containerName: '',
                fileName: ''
              };
            } else {
              tempPicture = {
                containerName: user.image.containerName,
                fileName: user.image.fileName
              };
            }
            const retObj: IUserListModel = {
              id: user.id,
              email: user.email,
              name: user.name,
              phoneNumber: user.phoneNumber,
              picture: tempPicture
            };
            return retObj;
          })
        })),
        employees: dto.employees
      }))
    );
  }

  getCompanyList(filter: ICompanyFilterModel): Observable<PagedData<ICompanyListModel>> {
    return this.companyClient.get(
      filter.name,
      filter.companyType,
      filter.email,
      filter.address,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize).pipe(
        map((dto: PagedListDtoOfCompanyListDto): PagedData<ICompanyListModel> => ({
          items: dto.data.map(x => ({
            id: x.id,
            name: x.name,
            contactPerson: x.contactPerson,
            address: x.address,
            companyType: x.companyType.companyType.toString()
          })),
          pageIndex: filter.pager.pageIndex,
          pageSize: filter.pager.pageSize,
          totalCount: dto.totalCount
        }))
      );
  }

  putCompany(id: number, company: ICompanyDetailsModel): Observable<void> {
    const dto = new CompanyUpdateDto({
      address: new AddressCreateDto({
        address: company.address.address,
        city: company.address.city,
        countryId: company.address.countryId,
        postCode: company.address.postCode
      }),
      companyTypeId: company.companyTypeId,
      contactPersonId: company.contactPerson.id,
      taxNumber: company.taxNumber,
      registerNumber: company.registerNumber,
      openingHours: company.openingHours.map((oh): CompanyDateRangeUpdateDto => {
        const fromArray = oh.from.split(':');
        const toArray = oh.to.split(':');
        const retObj = new CompanyDateRangeUpdateDto({
          dayTypeId: oh.dayTypeId,
          from: new Date(Date.UTC(2019, 1, 1, +fromArray[0], +fromArray[1])),
          to: new Date(Date.UTC(2019, 1, 1, +toArray[0], +toArray[1]))
        });
        return retObj;
      }),
      userTeams: company.userTeams.map((member): UserTeamUpdateDto => new UserTeamUpdateDto({
        id: member.id,
        name: member.name,
        userIds: member.users.map((user: IUserListModel): number => {
          return user.id;
        })
      }))
    });
    return this.companyClient.updateCompany(id, dto);
  }

  deleteCompany(id: number): Observable<void> {
    return this.companyClient.deleteCompany(id);
  }

  getCompanyTypes(): Observable<ICompanyTypeListModel[]> {
    return this.companyTypeClient.getCompanyTypes().pipe(
      map((dto: CompanyTypeListDto[]): ICompanyTypeListModel[] => {
        return dto.map((item: CompanyTypeListDto): ICompanyTypeListModel => ({
          companyType: item.companyType,
          id: item.id,
          translation: item.translation
        }));
      })
    );
  }

  getDayTypes(): Observable<IDayTypesListModel[]> {
    return this.dayTypeClient.getDayTypes().pipe(
      map((dto: DayTypeListDto[]): IDayTypesListModel[] => {
        return dto.map((item: DayTypeListDto): IDayTypesListModel => ({
          id: item.id,
          dayType: item.dayType,
          translation: item.translation
        }));
      })
    );
  }

  getCountries(): Observable<ICountriesListModel[]> {
    return this.countryClient.getCountries().pipe(
      map((dto: CountryListDto[]): ICountriesListModel[] => {
        return dto.map((country: CountryListDto): ICountriesListModel => ({
          code: country.code,
          id: country.id,
          translation: country.translation
        }));
      })
    );
  }
}
