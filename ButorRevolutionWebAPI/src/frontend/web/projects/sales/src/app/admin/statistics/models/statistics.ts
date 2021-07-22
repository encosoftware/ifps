import { PagerModel } from '../../../shared/models/pager.model';

// SalesPersonStatisticsDto
export interface SalesPersonStatisticsModel {
    summary?: SalesPersonSummaryViewModel | undefined;
    salesPersons?: SalesPersonListViewModel[] | undefined;
}

export interface SalesPersonSummaryViewModel {
    numberOfOffers: number;
    numberOfContracts: number;
    effeciency: number;
    summaryTotal?: PriceListModel | undefined;
}

export interface SalesPersonListViewModel {
    name?: string | undefined;
    numberOfOffers: number;
    numberOfContracts: number;
    efficiency: number;
    total?: PriceListModel | undefined;
}
export interface PriceListModel {
    value?: number;
    currencyId?: number;
    currency?: string;
}

export interface IStatisticsFilterViewModel {
    name?: string | null | undefined;
    from?: Date | null | undefined;
    to?: Date | null | undefined;
    orderings?: OrderingViewModel[] | null | undefined;
    pager?: PagerModel;
}

export interface OrderingViewModel {
    column?: string;
    isDescending: boolean;
}

