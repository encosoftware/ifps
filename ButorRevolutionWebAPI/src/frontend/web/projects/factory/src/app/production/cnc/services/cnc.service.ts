import { Injectable } from '@angular/core';
import { ApiCncClient, IPagedListDtoOfCNCListDto } from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';
import { ICncListFilterViewModel, ICncListViewModel } from '../models/cnc.model';

@Injectable({
    providedIn: 'root'
})
export class CncService {

    constructor(
        private cncClient: ApiCncClient
    ) { }

    getCncs(filter: ICncListFilterViewModel): Observable<PagedData<ICncListViewModel>> {
        const tempFrom = filter.estimatedStartTime ? new Date(filter.estimatedStartTime) : undefined;
        const tempTo = filter.estimatedStartTime ? new Date() : undefined;
        return this.cncClient.cNCList(filter.componentId, filter.material, filter.orderId,
            filter.workingNr, tempFrom, tempTo, filter.workerName,
            undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: IPagedListDtoOfCNCListDto): PagedData<ICncListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        componentId: x.componentName,
                        material: x.materialCode,
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
