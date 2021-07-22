import { IPagerModel } from '../../../shared/models/pager.model';

export interface IMaterialPackageListModel {
    id: number;
    code: string;
    description: string;
    size: number;
    supplierName: string;
    price: number;
    currency: string;
}

export interface IMaterialPackageFilterModel {
    code?: string;
    description?: string;
    size?: number;
    supplierName?: string;
    pager: IPagerModel;
}

export interface IMaterialPackageDetailsModel {
    id?: number;
    materialId: string;
    supplierId: number;
    price: number;
    currencyId: number;
    code: string;
    description: string;
    size: number;
}

export interface IMaterialDropdownModel {
    id: string;
    name: string;
}

export interface ISupplierDropdownModel {
    id: number;
    name: string;
}

export interface ICurrencyListViewModel {
    name: string;
    id: number;
}
