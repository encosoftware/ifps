import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';
import { IOptimizationListViewModel, IOptimizationListFilterViewModel } from '../models/optimization.model';
import { ApiOptimizationClient, IPagedListDtoOfOptimizationListDto, ApiOptimizationLayoutZipClient, ApiOptimizationScheduleZipClient } from '../../../shared/clients';

@Injectable({
    providedIn: 'root'
})
export class OptimizationService {

    constructor(
        private optimizationClient: ApiOptimizationClient,
        private optimizationLayoutZipClient: ApiOptimizationLayoutZipClient,
        private optimizationScheduleZipClient: ApiOptimizationScheduleZipClient
    ) { }

    getOptimizations(filter: IOptimizationListFilterViewModel): Observable<PagedData<IOptimizationListViewModel>> {
        const tempTo = filter.startedAtTo ? new Date(filter.startedAtTo) : undefined;
        const tempFrom = filter.startedAtFrom ? new Date(filter.startedAtFrom) : undefined;
        return this.optimizationClient.getOptimizations(filter.shiftNumber, filter.shiftLength, tempFrom,
            tempTo, undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: IPagedListDtoOfOptimizationListDto): PagedData<IOptimizationListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        shiftNumber: x.shiftNumber,
                        shiftLength: x.shiftLength,
                        startedAt: x.startedAt                        
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            );
    }

    getLayoutZipFile(id: string): Observable<any> {
        return this.optimizationLayoutZipClient.downloadLayoutAsZip(id);
    }

    getScheduleZipFile(id: string): Observable<any> {
        return this.optimizationScheduleZipClient.downloadScheduleAsZip(id);
    }
}
