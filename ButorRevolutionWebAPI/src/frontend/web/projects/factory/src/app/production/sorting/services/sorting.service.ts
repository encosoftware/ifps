import { Injectable } from '@angular/core';
import { ApiSortingsClient, PagedListDtoOfSortingListDto } from '../../../shared/clients';
import { ISortingListViewModel, ISortingListFilterViewModel } from '../models/sorting.model';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SortingService {

  constructor(
    private sortingClient: ApiSortingsClient
  ) { }

  getSortingList(filter: ISortingListFilterViewModel): Observable<PagedData<ISortingListViewModel>> {
    const tempFrom = filter.date ? new Date(filter.date) : undefined;
    const tempTo = filter.date ? new Date() : undefined;
    return this.sortingClient.getSortings(filter.unitId, filter.orderId, filter.workingNr, tempFrom, tempTo, filter.workerName,
      undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
        map((dto: PagedListDtoOfSortingListDto): PagedData<ISortingListViewModel> => ({
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
