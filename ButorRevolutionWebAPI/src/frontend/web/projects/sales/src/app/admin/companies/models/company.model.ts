import { IEmployeeModel } from './employee.model';
import { IUserTeamModel } from './group.model';
import { PagerModel } from '../../../shared/models/pager.model';
import { CompanyTypeEnum, DayTypeEnum } from '../../../shared/clients';
import { IPictureModel } from '../../materials/models/decorboards.model';

export interface ICompanyDetailsModel {
    id: number;
    name: string;
    companyTypeId?: number;
    taxNumber: string;
    registerNumber: string;
    contactPerson?: IContactPersonModel;
    address: ICompanyAddressModel;
    openingHours: IOpeningHoursModel[];
    employees: IEmployeeModel[];
    userTeams: IUserTeamModel[];
}

export interface ICompanyListModel {
    id: number;
    name: string | undefined;
    companyType: CompanyTypeEnum;
    address: ICompanyAddressModel | undefined;
    contactPerson?: IContactPersonModel | undefined;
}

export interface ICompanyDayTypeViewModel {
    id: number;
    dayType: DayTypeEnum;
    translation: string;
}

export interface ICompanyFilterModel {
    id?: number;
    name?: string | undefined;
    companyType?: CompanyTypeEnum;
    address?: string | undefined;
    email?: string | undefined;

    pager: PagerModel;
}

export interface ICompanyCreateModel {
    id?: number;
    name: string;
    companyTypeId: number;
    taxNumber: string;
    registerNumber: string;
    address: ICompanyAddressModel;
}

export interface ICompanyAddressModel {
    address?: string;
    postCode?: number;
    city?: string;
    countryId?: number;
}

export interface IContactPersonModel {
    id?: number;
    name?: string;
    phoneNumber?: string;
    email?: string;
    picture?: IPictureModel;
}

export interface IContactPersonFilterModel {
    name?: string;
    phoneNumber: string;
    email?: string;
}

export interface IOpeningHoursModel {
    dayTypeId: number;
    from: string;
    to: string;
}

export interface ICompanyTypeListModel {
    id: number;
    companyType: CompanyTypeEnum;
    translation?: string;
}

export interface IDayTypesListModel {
    id: number;
    dayType: DayTypeEnum;
    translation?: string;
}

export interface ICountriesListModel {
    id: number;
    code: string;
    translation: string;
}
