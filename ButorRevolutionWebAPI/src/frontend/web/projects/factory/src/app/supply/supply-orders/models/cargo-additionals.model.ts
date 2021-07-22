import { IMaterialPackageModel } from './cargo-materials.model';

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
