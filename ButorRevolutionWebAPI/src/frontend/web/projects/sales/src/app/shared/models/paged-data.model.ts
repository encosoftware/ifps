export interface PagedData<T> {
    items?: T[];
    totalCount?: number;
    pageIndex?: number;
    pageSize?: number;
}
