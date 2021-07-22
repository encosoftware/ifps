import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';
import { ApiCuttingsClient, IPagedListDtoOfCuttingsListDto } from '../../../shared/clients';
import { ICuttingListFilterViewModel, ICuttingListViewModel } from '../models/cuttings.model';

@Injectable({
    providedIn: 'root'
})
export class CuttingsService {

    constructor(
        private cuttingsClient: ApiCuttingsClient
    ) { }

    getCuttings(filter: ICuttingListFilterViewModel): Observable<PagedData<ICuttingListViewModel>> {
        const tempFrom = filter.estimatedStartTime ? new Date(filter.estimatedStartTime) : undefined;
        const tempTo = filter.estimatedStartTime ? new Date() : undefined;
        return this.cuttingsClient.cuttingsList(filter.machine, filter.material, filter.orderId,
            filter.workingNr, tempFrom, tempTo, filter.workerName,
            undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: IPagedListDtoOfCuttingsListDto): PagedData<ICuttingListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        machine: x.machine,
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
