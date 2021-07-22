export interface ICargoShippingViewModel {
    shippingCost: number;
    currencyId: number;
    shippingAddress: ICargoShippingAddressModel;
    note: string;
    vatIsChecked?: boolean;
}

export interface ICargoShippingAddressModel {
    countryId?: number;
    postCode: number;
    city: string;
    address: string;
}

export interface IDropDownViewModel {
    id: number;
    translation: string;
}
