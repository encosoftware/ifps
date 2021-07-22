export interface IStatisticsListModel {
    month: number;
    recurringCost: number;
    generalExpenseCost: number;
    income: number;
    profit: number;
    currency: string;
}

export interface IStatisticsFilterModel {
    year?: number;
}

export interface IStatisticsDateDropdown {
    // id: number;
    id: Date;
    name: string;
}
