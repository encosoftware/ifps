import { IPagerModel } from '../../../shared/models/pager.model';
import { ImageDetailsViewModel } from '../../../core/models/auth';
import { IPictureModel } from '../../materials/models/worktops.model';
import { IPriceViewModel } from '../../../sales/orders/models/offer.models';

export interface IWFUListModel {
    id: number;
    furnitureUnitId: string;
    description: string;
    code: string;
    price: string;
}

export interface IWFUFilterModel {
    code?: string;
    description?: string;
    pager: IPagerModel;
}

export interface IWFUDetailsModel {
    furnitureUnitId: string;
    images?: IPictureModel[];
    price?: PriceListModel;

}
export interface IFurnitureUnitDropdownModel {
    id: string;
    code: string;
}
export interface ICurrenciesDropdownModel {
    id: number;
    name: string;
}
export interface PriceListModel {
    value?: number | undefined;
    currencyId?: number | undefined;
    currency?: string | undefined;
}
export interface FurnitureUnitForWebshopViewModel {
    code?: string | undefined;
    furnitureUnitId?: string;
    description?: string | undefined;
    imageDetailsDto?: IPictureModel[] | undefined;
    price?: PriceListModel;

}