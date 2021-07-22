import { IPagerModel } from '../../../shared/models/pager.model';

export interface ICellsListViewModel {
    id: number;
    name: string;
    stock: string;
    description: string;
}

export interface INewCellViewModel {
    stockId: number;
    name: string;
    description: string;
}

export interface ICellStockListModel {
    id: number;
    name: string;
}

export interface IStockDropDownViewModel {
    id: number;
    name: string;
}

export interface ICellFilterListViewModel {
    name?: string;
    stock?: string;
    description?: string;
    pager: IPagerModel;
}
