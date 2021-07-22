import { IPagerModel } from '../../../shared/models/pager.model';

export interface IEdgingMachinesListViewModel {
    id: number;
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandName: string;
}

export interface IEdgingMachinesFilterViewModel {
    machineName?: string;
    softwareVersion?: string;
    serialNumber?: string;
    code?: string;
    yearOfManufacture?: string;
    brandId?: number;
    pager: IPagerModel;
}

export interface IEdgingMachinesDetailsViewModel {
    id?: number;
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandId: number;
}

export interface IEdgingMachinesCreateViewModel {
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandId: number;
}

export interface ISupplierDropdownModel {
    id: number;
    name: string;
}
