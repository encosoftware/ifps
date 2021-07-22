export interface IPagerModel {
    pageIndex: number;
    pageSize: number;
    totalCount?: number;
}

export class PagerModel implements IPagerModel {
    pageIndex = 0;
    pageSize = 10;
    totalCount?: number;
}
