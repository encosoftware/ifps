import { IPagerModel } from '../../../shared/models/pager.model';

export interface ICuttingMachinesListViewModel {
    id: number;
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandName: string;
}

export interface ICuttingMachinesFilterViewModel {
    machineName?: string;
    softwareVersion?: string;
    serialNumber?: string;
    code?: string;
    yearOfManufacture?: string;
    brandId?: number;
    pager: IPagerModel;
}

export interface ICuttingMachinesDetailsViewModel {
    id?: number;
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandId: number;
}

export interface ICuttingMachinesCreateViewModel {
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
