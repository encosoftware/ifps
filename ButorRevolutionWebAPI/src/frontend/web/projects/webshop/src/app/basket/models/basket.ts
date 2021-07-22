import { PriceListModel } from '../../shared/models/shared';
import { IPictureModel } from '../../shared/models/image-slider';

export interface BasketDetailsViewModel {
    orderedFurnitureUnits?: OrderedFurnitureUnitListViewModel[] | undefined;
    subTotal?: PriceListModel;
    delieveryPrice?: PriceListModel;
}

export interface OrderedFurnitureUnitListViewModel {
    furnitureUnitId: string;
    quantity: number;
    furnitureUnitListDto?: FurnitureUnitListViewModel | undefined;
}

export interface FurnitureUnitListViewModel {
    id: string;
    code?: string;
    description?: string;
    category?: CategoryListModel;
    width: number;
    height: number;
    depth: number;
    sellPrice: PriceListModel;
    imageThumbnail?: IPictureModel;
}

export interface CategoryListModel  {
    id: number;
    name?: string | undefined;
}

export interface OrderedFurnitureUnitUpdateModel {
    quantity: number;
}

export interface ShippingServiceListViewModel  {
    id: number;
    description?: string;
    price?: PriceListModel;

}
