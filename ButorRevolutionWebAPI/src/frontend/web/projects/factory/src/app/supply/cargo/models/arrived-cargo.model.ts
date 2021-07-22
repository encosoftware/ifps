import {
    ICargoCompanyViewModel,
    ICargoShippingViewModel,
    ICargoPreviewListViewModel
} from '../../supply-orders/models/cargo-preview.model';

export interface IArrivedCargoViewModel {
    cargoId: string;
    status: string;
    stockedOn: string;
    bookedBy: string;
    created: string;
    supplier: string;
    totalGrossCost: number;
    contractingCompany: ICargoCompanyViewModel;
    supplierCompany: ICargoCompanyViewModel;
    shipping: ICargoShippingViewModel;
    items: ICargoPreviewListViewModel[];
    currency: string;
    shippingPrice: number;
    subtotal: number;
    vat: number;
    total: number;
}
