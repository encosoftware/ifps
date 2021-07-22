import { PagerModel } from '../../../shared/models/pager.model';

export interface IFoilsListViewModel {
    id: string;
    code: string;
    picture: IPictureModel;
    description: string;
    thickness: number;
    purchasingPrice: number;
    currency?: string;
    currencyId?: number;
    transactionPrice: number;
}

export interface IFoilsModel {
    id: string;
    code: string;
    description: string;
    picture: IPictureModel;
    thickness: number;
    purchasingPrice: number;
    currency?: string;
    currencyId?: number;
    transactionPrice: number;
}

export interface IFoilsFilterModel {
    code?: string;
    description?: string;

    pager: PagerModel;
}

export interface IPictureModel {
    containerName?: string;
    fileName?: string;
}
