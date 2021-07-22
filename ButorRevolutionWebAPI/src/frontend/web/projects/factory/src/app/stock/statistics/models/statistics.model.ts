export interface IStatisticsData {
    weekNumber: number;
    quantity: number;
}

export interface IStockStatistics {
    materialCode: string;
    data: IStatisticsData[];
}
