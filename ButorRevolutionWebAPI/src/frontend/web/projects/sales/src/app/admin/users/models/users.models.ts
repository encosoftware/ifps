import { PagerModel } from '../../../shared/models/pager.model';
import { AbsenceTypeEnum, DayTypeEnum, DivisionTypeEnum, LanguageTypeEnum } from '../../../shared/clients';

export interface IUsersListViewModel {
    id: number;
    name: string;
    role: string;
    status?: boolean;
    company: string;
    email: string;
    phone: string;
    addedOn: Date;
    addedOnString?: string;
    image: ImageDetailsViewModel;
}

export interface IUsersCreateListViewModel {
    name?: string;
    phoneNumber?: string;
    email?: string;
    roleId?: number;
}

export interface IUsersFilterViewModel {
    name?: string;
    role?: number;
    status?: boolean;
    company?: string;
    email?: string;
    phone?: string;
    addedOnTo?: Date;
    pager: PagerModel;
}

export interface IUserEditViewModel {
    basicInfo?: IUserBasicInfoViewModel;
    isEmployee?: boolean;
    isActivated?: boolean;
    claims?: number[];
    daysoff?: ICalendarViewModel;
    workingInfo?: IUserWorkingInfoViewModel;
    notifications?: IUserNotificationDataModel;

}

export interface IUserBasicInfoViewModel {
    id?: number;
    name: string;
    roles?: number[];
    status?: string;
    company?: number;
    companyList?: ICompanySelectList;
    address?: IAddressViewModel;
    email?: string;
    phone?: string;
    language?: LanguageTypeEnum;
    addedOn?: string;
    image?: ImageDetailsViewModel;
    isCompanyRequired?: boolean;
}
export interface ImageDetailsViewModel {
    containerName?: string;
    fileName?: string;
    src?: string;
}
export interface ICompanySelectList {
    id: number;
    name?: string;
}
export interface ILanguageSelectList {
    id: number;
    name?: string;
}

export interface IAddressViewModel {
    address?: string;
    postCode?: number;
    city?: string;
    countryId?: number;
}

export interface IUserWorkingInfoViewModel {
    usersOffices?: number[];
    usersOfficesList?: UsersOfficesModel[];
    supervisors?: number;
    supervisorsList?: IUserRole;
    supervisees?: IUserRole[];
    groups?: string[];
    discount?: { from: number, to: number };
    workingHours?: IUserWorkingHoursModel[];
}

export interface IUserWorkingHoursModel {
    day?: string;
    isChecked?: boolean;
    dayTypeId?: number;
    workingHour?: IUserWorkingHour[];
}

export interface IUserWorkingHour {
    dayTypeId: number;
    id: number;
    from: string | Date;
    to: string | Date;
}

export interface IModuleViewModel {
    name: string;
    description: string;
    division?: DivisionTypeEnum;
    claims: IClaimViewModel[];
}

export interface IClaimViewModel {
    id: number;
    name: string;
    division?: DivisionTypeEnum;
    isChecked?: boolean;
}

export interface ICalendarViewModel {
    sickLeave: AbsenceDayViewModel[];
    daysOff: AbsenceDayViewModel[];
    deleted?: Date[];
}

export interface IUserRole {
    id: number;
    name?: string;
}

export interface IUserNotificationViewModel {
    label: string;
    notifications: IUserNotificationModel[];
}
export interface IUserNotificationDataModel {
    notificationType: string[];
    eventTypeIds: number[];
}
export interface IUserNotificationModel {
    id?: number;
    isChecked?: boolean;
    name: string;
}

export interface AbsenceDayViewModel {
    id?: number;
    date: Date;
    absenceType?: AbsenceTypeEnum;
}

export interface UserSalesModel {
    id: number;
    name?: string;
    phoneNumber?: string;
    email?: string;
}

export interface CompanySelectModel {
    id: number;
    name: string;
}
export interface RolesSelectModel {
    id: number;
    name: string;
}
export interface CountryListSelectModel {
    id: number;
    name: string;
}
export interface UsersOfficesModel {
    id: number;
    name: string;
}

export interface DayTypeListModel {
    id: number;
    dayType: DayTypeEnum;
    translation: string;
    order: number;
}

export interface ActivatedFilterModel {
    label: string;
    value: boolean;
}

export interface AddedOnFilterModel {
    name: string;
    toDate: string;
}
