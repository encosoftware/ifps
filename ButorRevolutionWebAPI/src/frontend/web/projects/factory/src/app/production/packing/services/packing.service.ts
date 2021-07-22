import { Injectable } from '@angular/core';
import { ApiPackingsClient, PagedListDtoOfPackingListDto } from '../../../shared/clients';
import { IPackingListFilterViewModel, IPackingListViewModel } from '../models/packing.model';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PackingService {

  constructor(
    private packingClient: ApiPackingsClient
  ) { }

  getPackingList(filter: IPackingListFilterViewModel): Observable<PagedData<IPackingListViewModel>> {
    const tempFrom = filter.date ? new Date(filter.date) : undefined;
    const tempTo = filter.date ? new Date() : undefined;
    return this.packingClient.getPackings(filter.unitId, filter.orderId, filter.workingNr, tempFrom, tempTo, filter.workerName,
      undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
        map((dto: PagedListDtoOfPackingListDto): PagedData<IPackingListViewModel> => ({
          items: dto.data.map(x => ({
            id: x.id,
            unitId: x.unitName,
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
