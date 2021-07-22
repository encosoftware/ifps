import { PagerModel } from '../../../shared/models/pager.model';

export interface IGeneralRulesListViewModel {
    id: number;
    name: string;
    amountValue: number;
    amountCurrency: string;
    interval: number;
    frequency: string;
    startDate: Date;
}

export interface IGeneralRulesListFilterViewModel {
    name?: string;
    amount?: number;
    frequencyFrom?: number;
    frequencyTo?: number;
    frequencyTypeId?: number;
    startDate?: Date;
    pager: PagerModel;
}

export interface INewGeneralRuleViewModel {
    name: string;
    frequencyTypeId: number;
    startDate: Date;
    frequency: number;
    amountValue: number;
    amountCurrencyId: number;
    isFixed: boolean;
}
