import { ICargoMaterialsViewModel } from './cargo-materials.model';
import { ICargoAdditionalsViewModel } from './cargo-additionals.model';
import { ICargoShippingViewModel } from './cargo-shipping.model';

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
