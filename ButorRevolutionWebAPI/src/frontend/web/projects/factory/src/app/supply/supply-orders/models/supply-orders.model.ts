import { PagerModel } from '../../../shared/models/pager.model';

export interface ISupplyOrderListViewModel {
    id: number;
    orderId: string;
    workingNumber: string;
    material: string;
    name: string;
    amount: number;
    supplier: ISupplyDropModel[];
    deadline: string;
    isChecked?: boolean;
}

export interface ISupplyOrderFilterViewModel {
    orderId?: string;
    workingNumber?: string;
    material?: string;
    name?: string;
    supplier?: number;
    deadline?: string;
    pager: PagerModel;
}

export interface ISupplyDropdownModel {
    id: string;
    name: string;
}

export interface ISupplyDropModel {
    id: number;
    name: string;
}
