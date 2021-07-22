import { PagerModel } from '../../../shared/models/pager.model';
import { CargoStatusEnum } from '../../../shared/clients';

export interface ICargoListViewModel {
    id: number;
    cargoId: string;
    status: string;
    statusEnum: CargoStatusEnum;
    statusId: number;
    created: string;
    supplier: string;
    bookedBy: string;
    totalCost: number;
    currency: string;
}

export interface ICargoFilterModel {
    cargoId?: string;
    status?: number;
    created?: string;
    supplier?: number;
    bookedBy?: string;

    pager: PagerModel;
}

export interface ICargoDetailsViewModel {
    cargoId: string;
    status: string;
    bookedBy: string;
    created: string;
    contractingParty: string;
    supplier: string;
    netCost: number;
    currency: string;
    vat: number;
    totalCost: number;
    isArrived: boolean;
    productList: IProductListViewModel[];
}

export interface IProductListViewModel {
    id: number;
    material: string;
    name: string;
    orderedAmount: number;
    missing: number;
    refused: number;
    isChecked: boolean;
    packageCode?: string;
    packageName?: string;
    packageSize?: number;
}
