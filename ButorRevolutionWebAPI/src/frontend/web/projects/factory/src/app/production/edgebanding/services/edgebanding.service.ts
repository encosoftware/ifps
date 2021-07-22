import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';
import { ApiEdgebandingClient, IPagedListDtoOfEdgebandingListDto } from '../../../shared/clients';
import { IEdgebandingListFilterViewModel, IEdgebandingListViewModel } from '../models/edgebanding.model';

@Injectable({
    providedIn: 'root'
})
export class EdgebandingService {

    constructor(
        private edgebandingClient: ApiEdgebandingClient
    ) { }

    getEdgebandings(filter: IEdgebandingListFilterViewModel): Observable<PagedData<IEdgebandingListViewModel>> {
        const tempFrom = filter.estimatedStartTime ? new Date(filter.estimatedStartTime) : undefined;
        const tempTo = filter.estimatedStartTime ? new Date() : undefined;
        return this.edgebandingClient.edgebandingList(filter.componentId, filter.orderId,
            filter.workingNr, tempFrom, tempTo, filter.workerName,
            undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: IPagedListDtoOfEdgebandingListDto): PagedData<IEdgebandingListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        componentId: x.componentName,
                        orderId: x.orderName,
                        workingNr: x.workingNumber,
                        estimatedStartTime: x.estimatedStartTime,
                        estimatedProcessTime: x.estimatedProcessTime,
                        actualProcessTime: x.actualProcessTime,
                        workerName: x.workerNames,
                        isStarted: x.isStarted,
                        isShowedButton: x.isShowedButton
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            );
    }

}
