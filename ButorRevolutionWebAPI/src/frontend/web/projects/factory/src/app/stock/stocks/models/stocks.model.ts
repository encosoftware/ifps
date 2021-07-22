import { IPagerModel } from '../../../shared/models/pager.model';

export interface IStoragesListViewModel {
    id: number;
    name: string;
    address: string;
}

export interface INewStorageViewModel {
    name: string;
    address: IAddressViewModel;
}

export interface IAddressViewModel {
    country: number;
    postCode: number;
    city: string;
    address: string;
}

export interface ICountryListModel {
    id: number;
    translation: string;
}

export interface IStorageFilterListViewModel {
    name?: string;
    address?: string;
    pager: IPagerModel;
}
