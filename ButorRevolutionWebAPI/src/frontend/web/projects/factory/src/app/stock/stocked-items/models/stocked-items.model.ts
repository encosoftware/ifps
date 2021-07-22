import { PagerModel } from '../../../shared/models/pager.model';

export interface IStockedItemListViewModel {
    id: number;
    description: string;
    code: string;
    cellName: string;
    cellMeta: string;
    quantity: number;
    order?: string;
    count?: any;
    isSelected: boolean;
}

export interface IStockedItemFilterListViewModel {
    description?: string;
    code?: string;
    cellName?: string;
    cellMeta?: string;
    quantity?: number;
    order?: string;
    pager: PagerModel;
}

export interface INewStockedItemViewModel {
    codeId: number;
    cellId: number;
    amount: number;
}

export interface ICellDropDownViewModel {
    name: string;
    id: number;
}

export interface ICodeDropDownViewModel {
    name: string;
    id: number;
}

export interface IOrderDropDownViewModel {
    name: string;
    id: string;
}
