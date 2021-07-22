import { PagerModel } from '../../../shared/models/pager.model';

export interface IDecorboardListViewModel {
    id: string;
    code: string;
    description: string;
    categoryId: number;
    category?: string;
    length: number;
    width: number;
    thickness: number;
    purchasingPrice: number;
    currency?: string;
    currencyId?: number;
    transactionPrice: number;
    picture: IPictureModel;
}

export interface IDecorboardModel {
    id: string;
    code: string;
    description: string;
    category?: string;
    categoryId: number;
    supplierId: number;
    picture: IPictureModel;
    length: number;
    width: number;
    thickness: number;
    fiberHeading: boolean;
    purchasingPrice: number;
    currency?: string;
    currencyId?: number;
    transactionPrice: number;
}

export interface IDecorboardFilterModel {
    code?: string;
    description?: string;
    category?: string;
    categoryId?: number;

    pager: PagerModel;
}

export interface IGroupingModel {
    id: number;
    name: string;
}

export interface IPictureModel {
    containerName?: string;
    fileName?: string;
}
