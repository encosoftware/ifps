import { PagerModel } from '../../../shared/models/pager.model';

export interface ICargoListViewModel {
    id: number;
    cargoId: string;
    status: string;
    statusEnum: string;
    arrivedOn: Date;
    supplier: string;
    bookedBy: string;
}

export interface ICargoFilterListViewModel {
    cargoId?: string;
    status?: number;
    arrivedOn?: Date;
    supplier?: number;
    bookedBy?: string;
    pager: PagerModel;
}

export interface ICargoEditViewModel {
    basics: ICargoHeaderViewModel;
    details: ICargoDetailsViewModel;
}

export interface ICargoHeaderViewModel {
    cargoId: string;
    status: string;
    created: Date;
    supplier: string;
    bookedBy: string;
    stockedOn: Date;
    grossCost: number;
    grossCostCurrency: string;
}

export interface ICargoDetailsViewModel {
    cargoId: string;
    supplier: ICargoSupplierViewModel;
    contract: ICargoContractViewModel;
    shipping: ICargoShippingViewModel;
    products: ICargoProductListViewModel[];
}

export interface ICargoSupplierViewModel {
    name: string;
    address: string;
    email: string;
    person: string;
    phone: string;
}

export interface ICargoContractViewModel {
    name: string;
    address: string;
    email: string;
    person: string;
    phone: string;
}

export interface ICargoShippingViewModel {
    address: string;
    name: string;
    phone: string;
    notes: string;
}

export interface ICargoProductListViewModel {
    id: number;
    material: string;
    name: string;
    workingNr: string;
    arrived: number;
    refused?: number;
    cellId: number;
}

export interface ICargoSupplierCompanyViewModel {
    name: string;
    id: number;
}

export interface ICargoStatusViewModel {
    name: string;
    id: number;
} 

export interface ICargoBookedByViewModel {
    name: string;
    id: number;
}


