import { Injectable } from '@angular/core';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ICellsListViewModel, ICellFilterListViewModel, INewCellViewModel, IStockDropDownViewModel } from '../models/cells.model';
import {
  ApiStoragecellsClient,
  PagedListDtoOfStorageCellListDto,
  StorageCellCreateDto,
  StorageCellUpdateDto,
  ApiStoragesNamesClient,
  StorageNameListDto
} from '../../../shared/clients';

@Injectable({
  providedIn: 'root'
})
export class CellsService {

  constructor(
    private cellClient: ApiStoragecellsClient,
    private stockClient: ApiStoragesNamesClient
  ) { }

  getStockDropDown(): Observable<IStockDropDownViewModel[]> {
    return this.stockClient.getStorageNameList().pipe(map(res => res.map(this.stocksToViewModel)));
  }

  private stocksToViewModel(dto: StorageNameListDto): IStockDropDownViewModel{
    return {id: dto.id, name: dto.name};
  }

  getCells = (filter: ICellFilterListViewModel): Observable<PagedData<ICellsListViewModel>> =>
    this.cellClient.getStorageCells(
      filter.name,
      filter.stock,
      filter.description,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfStorageCellListDto): PagedData<ICellsListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          name: x.name,
          stock: x.storageName,
          description: x.description
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    )

  postCell(model: INewCellViewModel): Observable<number> {
    let dto = new StorageCellCreateDto({
      storageId: model.stockId,
      name: model.name,
      description: model.description
    });
    return this.cellClient.createStorageCell(dto);
  }

  putCell(id: number, model: INewCellViewModel): Observable<void> {
    let dto = new StorageCellUpdateDto({
      name: model.name,
      storageId: model.stockId,
      description: model.description,
    });
    return this.cellClient.updateStorageCell(id, dto);
  }

  deleteCell(id: number): Observable<void> {
    return this.cellClient.deleteStorageCell(id);
  }

  getCell(id: number): Observable<INewCellViewModel> {
    return this.cellClient.getStorageCell(id).pipe(map(res => ({
      stockId: res.storageId,
      name: res.name,
      description: res.description
    })));
  }


}
