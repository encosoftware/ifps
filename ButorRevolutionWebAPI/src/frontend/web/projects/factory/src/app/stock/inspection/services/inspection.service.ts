import { Injectable } from '@angular/core';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
    IInspectionListViewModel,
    IInspectionFilterListViewModel,
    IInspectionViewModel,
    INewInspectionViewModel,
    IInspectionProductListViewModel,
    IStocksDropDownViewModel,
    IUserDropDownViewModel
} from '../models/inspection.model';
import {
    ApiInspectionsClient,
    IPagedListDtoOfInspectionListDto,
    InspectionCreateDto,
    InspectionUpdateDto,
    ApiInspectionsReportsClient,
    ReportDetailsDto,
    ReportUpdateDto,
    StockReportUpdateDto,
    ApiStoragesNamesClient,
    StorageNameListDto,
    ApiUsersDropdownClient,
    BookedByDropdownListDto,
    ApiInspectionsReportsCloseClient
} from '../../../shared/clients';


@Injectable({
    providedIn: 'root'
})
export class InspectionService {

    constructor(
        private inspectionClient: ApiInspectionsClient,
        private inspectionReportClient: ApiInspectionsReportsClient,
        private inspectionReportCloseClient: ApiInspectionsReportsCloseClient,
        private stockClient: ApiStoragesNamesClient,
        private userClient: ApiUsersDropdownClient
    ) { }

    getStocks(): Observable<IStocksDropDownViewModel[]> {
        return this.stockClient.getStorageNameList().pipe(map(res => res.map(this.stocksDropDownToViewModel)));
    }

    private stocksDropDownToViewModel(dto: StorageNameListDto): IStocksDropDownViewModel {
        return { id: dto.id, name: dto.name };
    }

    getUsersDropDown(): Observable<IUserDropDownViewModel[]> {
        return this.userClient.bookedByDropdownList().pipe(map(res => res.map(this.userDropDownToViewModel)));
    }

    private userDropDownToViewModel(dto: BookedByDropdownListDto): IStocksDropDownViewModel {
        return { id: dto.id, name: dto.name };
    }

    getInspections = (filter: IInspectionFilterListViewModel): Observable<PagedData<IInspectionListViewModel>> =>
        this.inspectionClient.getInspections(
            filter.date,
            filter.stock,
            filter.report,
            filter.delegation,
            undefined,
            filter.pager.pageIndex,
            filter.pager.pageSize
        ).pipe(
            map((dto: IPagedListDtoOfInspectionListDto): PagedData<IInspectionListViewModel> => ({
                items: dto.data.map(x => ({
                    id: x.id,
                    date: x.inspectedOn,
                    stock: x.storageName,
                    report: x.reportName,
                    delegation: x.delegationNames,
                    isClosed: x.reportIsClosed
                })),
                pageIndex: filter.pager.pageIndex,
                pageSize: 15,
                totalCount: dto.totalCount
            }))
        )

    deleteInspection(id: number): Observable<void> {
        return this.inspectionClient.deleteInspection(id);
    }

    postInspection(model: INewInspectionViewModel): Observable<number> {
        let dto = new InspectionCreateDto({
            storageId: model.storageId,
            reportName: model.reportName,
            inspectedOn: model.inspectedOn,
            delegationIds: model.delegationIds
        });
        return this.inspectionClient.createInspection(dto);
    }

    putInspection(id: number, model: INewInspectionViewModel): Observable<void> {
        let dto = new InspectionUpdateDto({
            storageId: model.storageId,
            reportName: model.reportName,
            inspectedOn: new Date(Date.UTC(model.inspectedOn.getFullYear(), model.inspectedOn.getMonth(), model.inspectedOn.getDate())),
            delegationIds: model.delegationIds
        });
        return this.inspectionClient.updateInspection(id, dto);
    }

    getInspection(id: number): Observable<INewInspectionViewModel> {
        return this.inspectionClient.getInspection(id).pipe(map(
            x => ({
                storageId: x.storageId,
                reportName: x.reportName,
                inspectedOn: x.inspectedOn,
                delegationIds: x.delegationIds
            })
        ));
    }

    getInspectionReport(id: number): Observable<IInspectionViewModel> {
        return this.inspectionReportClient.getInspectionReport(id).pipe(map(this.inspectionReportToViewModel));
    }

    private inspectionReportToViewModel(dto: ReportDetailsDto): IInspectionViewModel {
        let model: IInspectionViewModel = {
            isClosed: dto.inspection.reportIsClosed,
            basicInfo: {
                date: dto.inspection.inspectedOn,
                stock: dto.inspection.storageName,
                delegation: dto.inspection.delegationNames
            },
            products: []
        };
        for (let product of dto.stockReports) {
            let tmp: IInspectionProductListViewModel = {
                id: product.id,
                description: product.packageDescription,
                code: product.packageCode,
                cellName: product.storageCellName,
                cellMeta: product.storageCellMetadata,
                quantity: product.quantity
            };
            if (product.missingAmount !== undefined) {
                if (product.missingAmount === 0) {
                    tmp.isCorrect = true;
                } else {
                    tmp.isCorrect = false;
                    tmp.missing = product.missingAmount;
                }
            }
            model.products.push(tmp);
        }
        return model;
    }

    putInspectionReport(id: number, inspectionItems: IInspectionProductListViewModel[]): Observable<void> {
        let reportDto = new ReportUpdateDto({
            stockReports: []
        });
        for (let item of inspectionItems) {
            if (item.isCorrect === true) {
                reportDto.stockReports.push(new StockReportUpdateDto({
                    id: item.id,
                    missingAmount: 0
                }));
            } else {
                reportDto.stockReports.push(new StockReportUpdateDto({
                    id: item.id,
                    missingAmount: item.missing
                }));
            }
        }
        return this.inspectionReportClient.updateInspectionReport(id, reportDto);
    }

    closeInspectionReport(id: number): Observable<void> {
        return this.inspectionReportCloseClient.closeInspectionReport(id);
    }

}
