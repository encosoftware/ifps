export interface IImageDetailsModel {
    containerName?: string;
    fileName?: string;
}

export interface PriceListModel {
    value?: number | undefined;
    currencyId?: number | undefined;
    currency?: string | undefined;
}

export interface BasketCreateModel {
    customerId?: number;
    orderedFurnitureUnit?: OrderedFurnitureUnitModel[];
    deliveryPrice?: PriceListModel | undefined;
    serviceId?: number;

}

export interface OrderedFurnitureUnitModel {
    furnitureUnitId: string;
    quantity: number;
}
