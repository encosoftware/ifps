import { PriceListModel } from '../../shared/models/shared';
import { IPictureModel } from '../../shared/models/image-slider';

export interface WebshopOrdersListViewModel {
    id: string;
    orderName?: string | undefined;
    date: Date;
    total?: PriceListModel | undefined;
}

export interface WebshopOrdersDetailsViewModel {
    orderedFurnitureUnits?: WebshopOrdersOfuDetailsViewModel[] | undefined;
    total?: WebshopOrdersPriceDetailsViewModel | undefined;
}

export interface WebshopOrdersOfuDetailsViewModel {
    name?: string | undefined;
    description?: string | undefined;
    width: number;
    height: number;
    depth: number;
    quantity: number;
    unitPrice?: PriceListModel | undefined;
    subTotal?: PriceListModel | undefined;
    image?: IPictureModel | undefined;
}

export interface WebshopOrdersPriceDetailsViewModel  {
    subtotal?: PriceListModel | undefined;
    deliveryPrice?: PriceListModel | undefined;
}

