import { PagerModel } from '../../../shared/models/pager.model';

export interface IWorktopListViewModel {
    id: string;
    code: string;
    description: string;
    category?: string;
    categoryId?: number;
    length: number;
    width: number;
    thickness: number;
    purchasingPrice: number;
    purchasingCurrency?: string;
    transactionPrice: number;
    picture: IPictureModel;
}

export interface IWorktopModel {
    id: string;
    code: string;
    description: string;
    category?: string;
    categoryId: number;
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

export interface IWorktopFilterModel {
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
