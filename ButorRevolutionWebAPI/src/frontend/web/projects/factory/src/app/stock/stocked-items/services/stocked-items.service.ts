import { Injectable } from '@angular/core';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import {
    IStockedItemListViewModel,
    IStockedItemFilterListViewModel,
    INewStockedItemViewModel,
    ICellDropDownViewModel,
    ICodeDropDownViewModel,
    IOrderDropDownViewModel
} from '../models/stocked-items.model';
import {
    ApiStocksClient,
    PagedListDtoOfStockListDto,
    StockCreateDto,
    StockUpdateDto,
    ApiStocksEjectClient,
    ApiStocksReserveClient,
    StockQuantityDto,
    StockReserveDto,
    ApiStoragecellsDropdownClient,
    StorageCellDropdownListDto,
    ApiMaterialpackagesClient,
    MaterialPackageCodeListDto,
    ApiOrdersClient,
    OrderNameListDto,
    ApiOrdersNamesClient,
    ApiMaterialpackagesDropdownClient,
    ApiStocksExportClient
} from '../../../shared/clients';

@Injectable({
    providedIn: 'root'
})
export class StockedItemsService {

    constructor(
        private stockedItemClient: ApiStocksClient,
        private stockedItemEjectClient: ApiStocksEjectClient,
        private stockedItemReservedClient: ApiStocksReserveClient,
        private cellClient: ApiStoragecellsDropdownClient,
        private materialPackageDropdownClient: ApiMaterialpackagesDropdownClient,
        private orderNamesClient: ApiOrdersNamesClient,
        private csvClient: ApiStocksExportClient
    ) { }

    private dataSource = new BehaviorSubject<IStockedItemListViewModel[]>([]);
    data = this.dataSource.asObservable();

    downloadCsv(filter: IStockedItemFilterListViewModel): Observable<any> {
        return this.csvClient.exportCsv(filter.description, filter.code, filter.cellName,
            filter.cellMeta, filter.quantity, undefined, undefined,
            undefined);
    }

    getCells(): Observable<ICellDropDownViewModel[]> {
        return this.cellClient.storageCellDropdownList().pipe(map(res => res.map(this.cellDropDownToViewModel)));
    }

    private cellDropDownToViewModel(dto: StorageCellDropdownListDto): ICellDropDownViewModel {
        return { name: dto.name, id: dto.id };
    }

    getCodes(): Observable<ICodeDropDownViewModel[]> {
        return this.materialPackageDropdownClient.getPackageCodes().pipe(map(res => res.map(this.codeDropDownToViewModel)));
    }

    private codeDropDownToViewModel(dto: MaterialPackageCodeListDto): ICodeDropDownViewModel {
        return { id: dto.id, name: dto.code };
    }

    getOrders(): Observable<IOrderDropDownViewModel[]> {
        return this.orderNamesClient.getOrderNames().pipe(map(res => res.map(this.orderDropDownToViewModel)));
    }

    private orderDropDownToViewModel(dto: OrderNameListDto): IOrderDropDownViewModel {
        return { id: dto.id, name: dto.name };
    }

    updatedDataSelection(data: IStockedItemListViewModel[]) {
        this.dataSource.next(data);
    }

    getStockedItems = (filter: IStockedItemFilterListViewModel): Observable<PagedData<IStockedItemListViewModel>> =>
        this.stockedItemClient.getStocks(filter.description, filter.code, filter.cellName,
            filter.cellMeta, filter.quantity, undefined, filter.pager.pageIndex,
            filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfStockListDto): PagedData<IStockedItemListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        description: x.packageDescription,
                        code: x.packageCode,
                        cellName: x.storageCellName,
                        cellMeta: x.storageCellMetadata,
                        quantity: x.quantity,
                        isSelected: false,
                        order: x.orderName
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            )

    deleteStockedItem(id: number): Observable<void> {
        return this.stockedItemClient.deleteStock(id);
    }

    postStockedItem(model: INewStockedItemViewModel): Observable<number> {
        let dto = new StockCreateDto({
            packageId: model.codeId,
            storageCellId: model.cellId,
            quantity: model.amount
        });
        return this.stockedItemClient.createStock(dto);
    }

    getStockedItem(id: number): Observable<INewStockedItemViewModel> {
        return this.stockedItemClient.getStock(id).pipe(map(
            res => ({
                codeId: res.packageId,
                cellId: res.storageCellId,
                amount: res.quantity
            })
        ));
    }

    putStockedItem(id: number, model: INewStockedItemViewModel): Observable<void> {
        let dto = new StockUpdateDto({
            packageId: model.codeId,
            storageCellId: model.cellId,
            quantity: model.amount
        });
        return this.stockedItemClient.updateStock(id, dto);
    }

    postEjectStockedItems(model: IStockedItemListViewModel[]): Observable<void> {
        let dto = [];
        for (let item of model) {
            let stock = new StockQuantityDto({
                id: item.id,
                quantity: item.count
            });
            dto.push(stock);
        }
        return this.stockedItemEjectClient.ejectStock(dto);
    }

    postReserveStockedItems(id: string, model: IStockedItemListViewModel[]): Observable<void> {
        let dto = new StockReserveDto({
            orderId: id,
            stockQuantities: []
        });
        for (let item of model) {
            let stock = new StockQuantityDto({
                id: item.id,
                quantity: item.count
            });
            dto.stockQuantities.push(stock);
        }
        return this.stockedItemReservedClient.reserveStock(dto);
    }

}
