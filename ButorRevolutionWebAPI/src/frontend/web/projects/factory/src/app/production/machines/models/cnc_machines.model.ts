import { IPagerModel } from '../../../shared/models/pager.model';

export interface ICncMachinesListViewModel {
    id: number;
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandName: string;
}

export interface ICncMachinesFilterViewModel {
    machineName?: string;
    softwareVersion?: string;
    serialNumber?: string;
    code?: string;
    yearOfManufacture?: number;
    brandId?: number;
    pager: IPagerModel;
}

export interface ICncMachinesDetailsViewModel {
    id?: number;
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandId: number;
    sharedFolderPath: string;
}

export interface ICncMachinesCreateViewModel {
    machineName: string;
    softwareVersion: string;
    serialNumber: string;
    code: string;
    yearOfManufacture: number;
    brandId: number;
    sharedFolderPath: string;
}

export interface ISupplierDropdownModel {
    id: number;
    name: string;
}
