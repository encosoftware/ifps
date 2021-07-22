import { PagerModel } from '../../../shared/models/pager.model';

export interface IGeneralExpensesListViewModel {
    id: number;
    name: string;
    amountValue: number;
    amountCurrency: string;
    paymentDate: Date;
}

export interface IGeneralExpensesListFilterViewModel {
    name?: string;
    amount?: number;
    paymentDate?: Date;
    pager: PagerModel;
}

export interface INewGeneralExpenseViewModel {
    name: string;
    frequencyTypeId: number;
    paymentDate: Date;
    interval: number;
    amountValue: number;
    amountCurrencyId: number;
}

export interface ICurrencyListViewModel {
    name: string;
    id: number;
}

export interface IFrequencyListViewModel {
    translation: string;
    id: number;
}

export interface IRecurringCostViewModel {
    id: number;
    name: string;
    amount: number;
    currency: number;
}
