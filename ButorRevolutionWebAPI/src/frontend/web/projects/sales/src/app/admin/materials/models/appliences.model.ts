import { PagerModel } from '../../../shared/models/pager.model';

export interface IAppliancesListViewModel {
    id: string;
    code: string;
    description: string;
    categoryId?: number;
    brand?: string;
    brandId?: number;
    picture: IPictureModel;
    hanaCode: string;
    purchasingPrice: number;
    purchasingCurrencyId?: number;
    purchasingCurrency?: string;
    sellPrice: number;
    sellingCurrencyId?: number;
    sellingCurrency?: string;
}

export interface IAppliancesModel {
    id: string;
    code: string;
    description: string;
    categoryId?: number;
    picture: IPictureModel;
    brand?: string;
    brandId?: number;
    hanaCode: string;
    purchasingPrice: number;
    purchasingCurrencyId?: number;
    sellPrice: number;
    sellingCurrencyId?: number;
}

export interface IAppliancesFilterModel {
    code?: string;
    description?: string;
    categoryId?: number;
    category?: string;
    brand?: string;
    hanaCode?: string;

    pager: PagerModel;
}

export interface IPictureModel {
    containerName?: string;
    fileName?: string;
}

export interface IBrandListModel {
    id: number;
    name: string;
}
