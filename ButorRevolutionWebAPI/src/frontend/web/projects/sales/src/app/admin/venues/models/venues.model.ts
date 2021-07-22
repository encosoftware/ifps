import { IPagerModel } from '../../../shared/models/pager.model';
import { LanguageTypeEnum, DayTypeEnum } from '../../../shared/clients';

export interface IVenueViewModel {
    id?: number;
    isActive: boolean;
    basicInfo: IVenueBasicInfoViewModel;
    meetingRooms: IVenueMeetingRoomViewModel[];
}

export interface INewVenueViewModel {
    name: string;
    phone: string;
    email: string;
    address: IVenueAddressViewModel;
}

export interface IVenueBasicInfoViewModel {
    name: string;
    phone: string;
    email: string;
    address: IVenueAddressViewModel;
    openingHours: IVenueOpeningHoursViewModel[];
}

export interface IVenueOpeningHoursViewModel {
    id: number;
    dayType: DayTypeEnum,
    from: string;
    to: string;
}

export interface ILanguageListViewModel {
    languageType: LanguageTypeEnum,
    translation: string;
}

export interface IVenueDayTypeViewModel {
    id: number;
    dayType: DayTypeEnum;
    translation: string;
}

export interface IVenueMeetingRoomViewModel {
    id?: number;
    name: string;
    location: string;
}

export interface IVenueAddressViewModel {
    country: number;
    postCode: number;
    city: string;
    address: string;
}

export interface IVenueListViewModel {
    id: number;
    name: string;
    rooms: number;
    address: string;
    phone: string;
    email: string;
}

export interface IVenueFilterListViewModel {
    name?: string;
    rooms?: number;
    address?: string;
    phone?: string;
    email?: string;
    pager: IPagerModel;
}

export interface IVenueCountriesListViewModel {
    id: number;
    code: string;
    translation: string;
}

