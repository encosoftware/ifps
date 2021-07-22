import { PagerModel } from '../../../shared/models/pager.model';

export interface IAccessoryListViewModel {
    id: string;
    code: string;
    description: string;
    categoryId?: number;
    picture: IPictureModel;
    structurallyOptional: boolean;
    optMount: boolean;
    purchasingPrice: number;
    currency?: string;
    currencyId?: number;
    transactionPrice: number;
}

export interface IAccessoryModel {
    id: string;
    code: string;
    description: string;
    category?: string;
    categoryId?: number;
    picture: IPictureModel;
    structurallyOptional: boolean;
    optMount: boolean;
    purchasingPrice: number;
    currency?: string;
    currencyId?: number;
    transactionPrice: number;
}

export interface IAccessoryFilterModel {
    code?: string;
    description?: string;
    structurallyOptional?: boolean;
    optMount?: boolean;
    categoryId?: number;
    category?: string;

    pager: PagerModel;
}

export interface IPictureModel {
    containerName?: string;
    fileName?: string;
}
