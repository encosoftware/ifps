import { PriceListModel } from '../../shared/models/shared';
import { IPictureModel } from '../../shared/models/image-slider';

export interface FurnitureUnitByWebshopDetailsViewModel {

name?: string;
furnitureUnitId?: string;
description?: string;
width?: number;
height?: number;
depth?: number;
price?: PriceListModel;
image?: IPictureModel[];

}

export interface FurnitureUnitByWebshopDetailsRecomendationModel {
    webshopFurnitureUnitId: number;
    name?: string | undefined;
    description?: string | undefined;
    width: number;
    height: number;
    depth: number;
    price?: PriceListModel;
    image?: IPictureModel;
}

export interface WebshopFurnitureUnitInBasketIdsViewModel {
    furnitureUnitIds?: string[];
    recommendedItemNum: number;
}
