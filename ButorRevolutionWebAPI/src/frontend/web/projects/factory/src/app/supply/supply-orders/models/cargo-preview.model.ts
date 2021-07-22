export interface ICargoPreviewModel {
    cargoId: string;
    contractingCompany: ICargoCompanyViewModel;
    supplierCompany: ICargoCompanyViewModel;
    shipping: ICargoShippingViewModel;
    items: ICargoPreviewListViewModel[];
    currency?: string;
    shippingPrice: number;
    assemblyPrice?: number;
    subtotal: number;
    vat: number;
    total: number;
}


export interface ICargoCompanyViewModel {
    companyName: string;
    address: IAddressPreviewModel;
    email: string;
    contactPersonName: string;
    phone: string;
}

export interface ICargoShippingViewModel {
    address: IAddressPreviewModel;
    name: string;
    phone: string;
    note: string;
}

export interface ICargoPreviewListViewModel {
    material: string;
    name: string;
    price: number;
    packageCode?: string;
    packageName?: string;
    packageSize?: number;
    currency?: string;
    amount: number;
    missing?: number;
    refused?: number;
    subTotal: number;
}

export interface IAddressPreviewModel {
    postCode: number;
    city: string;
    address: string;
}
