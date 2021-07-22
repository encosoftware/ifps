import { PagerModel } from '../../../shared/models/pager.model';
import { PriceListModel } from '../../../finance/orders-finances/models/customerOrders';

export interface ISupplyOrderListViewModel {
    id: number;
    orderId: string;
    workingNumber: string;
    material: string;
    name: string;
    amount: number;
    supplier: ISupplyDropModel[];
    deadline: string;
    isChecked?: boolean;
}

export interface ISupplyOrderFilterViewModel {
    orderId?: string;
    workingNumber?: string;
    material?: string;
    name?: string;
    supplier?: number;
    deadline?: string;

    pager: PagerModel;
}

export interface ISupplyDropdownModel {
    id: string;
    name: string;
}

export interface ISupplyDropModel {
    id: number;
    name: string;
}

export interface INewCargoModel {
    cargoName: string;
    bookedBy: string;
    created: string;
    supplier: string;
    contracting: string;
    netCost: number;
    netCostCurrency: string;
    netCostCurrencyId?: number;
    vat: number;
    vatCurrency: string;
    vatCurrencyId: number;
    totalGross: number;
    totalGrossCurrency: string;
    totalGrossCurrencyId?: number;
    materials?: ICargoMaterialsViewModel[];
    additionals?: ICargoAdditionalsViewModel[];
    shipping?: ICargoShippingViewModel;
}

export interface ICargoMaterialsViewModel {
    materialCode: string;
    name: string;
    materialPackages: IMaterialPackageModel[];
    //missingAmount?: number;
    orderedAmount?: number;
    stockedAmount?: number;
    requiredAmount?: number;
    minimumAmount?: number;
    advisedAmount?: number;
    actualPrice?: number;
    actualCurrency?: string;
    actualCurrencyId?: number;
    selectedPackageId?: number;
}

export interface IMaterialPackageModel {
    id: number;
    title?: string;
    packageSize: number;
    //packageUnit: SiUnitEnum; TODO: delete?
    price: number;
    currency: string;
    currencyId: number;
}

export interface ICargoAdditionalsViewModel {
    materialCode: string;
    name: string;
    materialPackages: IMaterialPackageModel[];
    //missingAmount?: number;
    orderedAmount?: number;
    stockedAmount?: number;
    requiredAmount?: number;
    minimumAmount?: number;
    advisedAmount?: number;
    actualPrice?: number;
    actualCurrency?: string;
    actualCurrencyId?: number;
    selectedPackageId?: number;
}

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



export interface SelectedRequiredMaterialsViewModel {
    requiredMaterialsIds?: number[] | undefined;
    supplierId: number;
    bookedById: number;
}

export interface TempCargoDetailsForRequiredMaterialsViewModel {
    cargoDetailsBeforeSaveCargo?: ICargoDetailBeforeSaveCargoViewModel | undefined;
    materials?: IMaterialsListViewModel[] | undefined;
    additionals?: IAdditionalsListViewModel[] | undefined;
}

export interface ICargoDetailBeforeSaveCargoViewModel {
    cargoName?: string | undefined;
    bookedBy?: string | undefined;
    createdOn: Date;
    contractingPartyName?: string | undefined;
    supplierName?: string | undefined;
    netCost?: PriceListModel | undefined;
    vat?: PriceListModel | undefined;
    totalGrossCost?: PriceListModel | undefined;
    vatValue: number;
}

export interface IMaterialsListViewModel {
    materialCode?: string | undefined;
    name?: string | undefined;
    materialPackages?: IMaterialPackageByMaterialsListViewModel[] | undefined;
    materialPackagesSelected?: IMaterialPackageByMaterialsListViewModel | undefined;
    requiredAmount: number;
    stockedAmount: number;
    minAmount: number;
    advisedAmount: number;
    underOrderAmount: number;
    orderdAmount?: number;
}

export interface IMaterialPackageByMaterialsListViewModel {
    id: number;
    name: string;
    packageSize: number;
    price?: PriceListModel | undefined;
}

export interface IAdditionalsListViewModel {
    materialCode?: string | undefined;
    name?: string | undefined;
    materialPackages?: IMaterialPackageByMaterialsListViewModel[] | undefined;
    materialPackagesSelected?: IMaterialPackageByMaterialsListViewModel | undefined;
    stockedAmount: number;
    minAmount: number;
    advisedAmount: number;
    underOrderAmount: number;
    orderdAmount?: number;

}

export interface CargoCreateViewModel {
    cargoName?: string | undefined;
    bookedById: number;
    supplierId: number;
    shippingCost?: PriceListModel | undefined;
    shippingAddress?: AddressCreateModel | undefined;
    notes?: string | undefined;
    vat?: PriceListModel | undefined;
    netCost?: PriceListModel | undefined;
    additionals?: OrderedMaterialPackageCreateMaterial[] | undefined;
    materials?: OrderedMaterialPackageCreateMaterial[] | undefined;
}

export interface AddressCreateModel {
    address?: string | undefined;
    postCode: number;
    city?: string | undefined;
    countryId: number;
}

export interface OrderedMaterialPackageCreateMaterial {
    packageId: number;
    orderedPackageNum: number;
}
