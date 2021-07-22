import { ServiceTypeEnum } from '../../../shared/clients';
import { ImageDetailsViewModel } from '../../../admin/users/models/users.models';

export interface IOfferDetailsModel {
    generalInformation: IOfferGeneralInformationModel;
    topCabinet?: IOfferGeneralCabinetModel;
    baseCabinet?: IOfferGeneralCabinetModel;
    tallCabinet?: IOfferGeneralCabinetModel;
    topCabinetList?: IOfferCabinetsModel[];
    baseCabinetList?: IOfferCabinetsModel[];
    tallCabinetList?: IOfferCabinetsModel[];
    appliancesList?: IOfferAppliancesModel[];
    accessoriesList?: IOfferAccessoriesModel[];
    prices: IOfferPricesModel;
    shippingService?: IShippingServiceDetailsModel;
    assmeblyService?: IServiceDetailsModel;
    installationService?: IServiceDetailsModel;
    isVatRequired: boolean;
}

export interface IOfferGeneralInformationModel {
    privatePerson?: boolean;
    companyRepresentative?: boolean;
    description?: string;
    budgetPrice: number;
    budgetCurrencyId: number;
}

export interface IOfferGeneralCabinetModel {
    height: number;
    depth: number;
    outerMaterialId: number;
    innerMaterialId: number;
    backPanelMaterialId: number;
    doorMaterialId: number;
    descrpition?: string;
}

export interface IPriceViewModel {
    id: number;
    name: string;
}

export interface IDecorboardsViewModel {
    id: string;
    code: string;
    parentName?: string;
    parentId?: number;
}

export interface IOfferCabinetsModel {
    id?: number;
    unitId?: string;
    name: string;
    code: string;
    width?: number;
    depth?: number;
    height?: number;
    price: number;
    priceCurrency: string;
    quantity: number;
    subtotal?: number;
    subtotalCurrency?: string;
    fileName?: string;
    containerName?: string;
    src?: string;
}

export interface IOfferAppliancesModel {
    id?: number;
    unitId?: string;
    name: string;
    code: string;
    price: number;
    priceCurrency: string;
    quantity: number;
    subtotal?: number;
    subtotalCurrency?: string;
    fileName?: string;
    containerName?: string;
    src?: string;
}

export interface IOfferAccessoriesModel {
    id?: number;
    name: string;
    code: string;
    price: number;
    priceCurrency: string;
    quantity: number;
    subtotal: number;
    subtotalCurrency: string;
    fileName?: string;
    containerName?: string;
    src?: string;
}

export interface IOfferPricesModel {
    shippingPrice?: number;
    shippingCurrency?: string;
    assemblyPrice?: number;
    assemblyCurrency?: string;
    productsPrice: number;
    productsCurrency?: string;
    vatPrice: number;
    vatCurrency?: string;
    totalPrice: number;
    installationPrice: number;
    installationCurrency?: string;
    totalCurrency: string;
    note?: string;
}

export interface IOfferFormPreviewModel {
    offerName: string;
    renderers?: ImageDetailsViewModel[];
    shippingInfo?: IOfferShippingInformationModel;
    topCabinetList?: IOfferCabinetsModel[];
    baseCabinetList?: IOfferCabinetsModel[];
    tallCabinetList?: IOfferCabinetsModel[];
    appliancesList?: IOfferCabinetsModel[];
    accessoriesList?: IOfferCabinetsModel[];
    prices?: IOfferPricesModel;
}

export interface IOfferShippingInformationModel {
    producerName: string;
    headquarter: string;
    contactPerson: string;
    producerEmail: string;
    producerPhone: string;
    customerName: string;
    shippingAddress: string;
    customerEmail: string;
    customerPhone: string;
}

export interface IFurnitureUnitDetailsModel {
    id: number;
    code: string;
    category: string;
    categoryId: number;
    description: string;
    depth: number;
    height: number;
    width: number;
    quiantity: number;
    totalPrice: number;
    totalCurrency: string;
    src?: string;
    fronts: IAdditionalUnitsModel[];
    corpuses: IAdditionalUnitsModel[];
}

export interface IAdditionalUnitsModel {
    id: string;
    name: string;
    boardMaterialId: string;
    width: number;
    height: number;
    amount: number;
    fileName: string;
    containerName: string;
    topFoilId: string;
    bottomFoilId: string;
    rightFoilId: string;
    leftFoilId: string;
    topFoil: string;
    bottomFoil: string;
    rightFoil: string;
    leftFoil: string;
}

export interface IShippingServiceDetailsModel {
    isChecked?: boolean;
    description: string;
    basicFeePrice?: number;
    basicFeeCurrency?: string;
    distanceServiceId: number;
    totalPrice?: number;
    totalCurrency?: string;
}

export interface IServiceDetailsModel {
    isChecked?: boolean;
    description: string;
    serviceId: number;
    basicFeePrice?: number;
    basicFeeCurrency?: string;
}

export interface IServiceDropdownModel {
    id: number;
    isChecked?: boolean;
    description: string;
    price: number;
    currency: string;
    type: ServiceTypeEnum;
}
