export interface ICorpusTrendsListViewModel {
    id: string;
    name: string;
    code: string;
    description: string;
    hasFiberDirection: boolean;
    category: string;
    image: string;
    ordersCount: number;
}

export interface IFrontTrendsListViewModel {
    id: string;
    name: string;
    code: string;
    description: string;
    hasFiberDirection: boolean;
    category: string;
    image: string;
    ordersCount: number;
}

export interface IFurnitureUnitTrendsListViewModel {
    id: string;
    code: string;
    description: string;
    category: string;
    image: string;
    ordersCount: number;
}

export interface ITrendFilterModel {
    intervalFrom?: Date;
    intervalTo?: Date;
    take?: number;
}
