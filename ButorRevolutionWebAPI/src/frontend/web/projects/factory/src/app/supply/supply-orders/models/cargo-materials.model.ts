
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
