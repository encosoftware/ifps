import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { parse } from 'date-fns';
import { map } from 'rxjs/operators';
import {
    ApiOrderschedulingsClient,
    IPagedListDtoOfOrderSchedulingListDto,
    OrderStateTypeDropdownListDto,
    OrderStateEnum,
    ApiOptimizationClient,
    ApiSortingstrategiesClient,
    SortingStrategyTypeListDto,
    OrdersForOptimizationDto,
    ApiOrdersClient,
    ApiRequiredmaterialsClient,
    ApiOrderstatustypesStatusesOrderschedulingClient,
    ApiConcretefurniturecomponentsClient,
    ConcreteFurnitureComponentInformationListDto
} from '../../../shared/clients';
import {
    IOrderSchedulingListFilterViewModel,
    IOrderSchedulingListViewModel,
    IOrderSchedulingStatusListViewModel,
    IProductionStatusDetailsViewModel,
    IOptimisationModel,
    IDropdownModel,
    ICfcInformationListViewModel
} from '../models/order-scheduling.model';

@Injectable({
    providedIn: 'root'
})
export class OrderSchedulingService {

    constructor(
        private orderSchedulingClient: ApiOrderschedulingsClient,
        private statusClient: ApiOrderstatustypesStatusesOrderschedulingClient,
        private optimisationClient: ApiOptimizationClient,
        private sortingStrategyClient: ApiSortingstrategiesClient,
        private reserveOrderClient: ApiOrdersClient,
        private orderMaterialClient: ApiRequiredmaterialsClient,
        private cfcClient: ApiConcretefurniturecomponentsClient
    ) { }

    getOrderSchedulings(filter: IOrderSchedulingListFilterViewModel): Observable<PagedData<IOrderSchedulingListViewModel>> {
        const tempTo = filter.deadline ? new Date(filter.deadline) : undefined;
        const tempFrom = filter.deadline ? new Date() : undefined;
        return this.orderSchedulingClient.orderSchedulingList(filter.orderId, filter.workingNr, filter.statusId,
            filter.completion, tempFrom, tempTo, undefined, filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: IPagedListDtoOfOrderSchedulingListDto): PagedData<IOrderSchedulingListViewModel> => ({
                    items: dto.data.map(x => ({
                        selectable: (x.currentStatus &&
                            (OrderStateEnum[x.currentStatus.state]).toString() ===
                            (OrderStateEnum.WaitingForScheduling).toString()) ? true : false,
                        // showTime: (x.currentStatus &&
                        //     (OrderStateEnum[x.currentStatus.state]).toString() !==
                        //     (OrderStateEnum.ProductionComplete).toString()) ? true : false,
                        showStatus: (x.currentStatus &&
                            (OrderStateEnum[x.currentStatus.state]).toString() ===
                            (OrderStateEnum.UnderProduction).toString()) ? true : false,
                        orderId: x.orderId,
                        orderName: x.orderName,
                        workingNr: x.workingNumber,
                        status: x.currentStatus ? x.currentStatus.translation : '',
                        // estimatedProcessTime: x.estimatedProcessTime,
                        deadline: x.deadline,
                        showReserve: x.isEnough && (OrderStateEnum[x.currentStatus.state]).toString() ===
                            (OrderStateEnum.UnderMaterialReservation).toString(),
                        showOrderMaterial: !x.isEnough && (OrderStateEnum[x.currentStatus.state]).toString() ===
                            (OrderStateEnum.UnderMaterialReservation).toString()
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            );
    }

    getOrderSchedulingStatuses(): Observable<IOrderSchedulingStatusListViewModel[]> {
        return this.statusClient.getOrderSchedulingOrderStates().pipe(map(res => res.map(this.orderSchedulingStatusesToViewModel)));
    }

    private orderSchedulingStatusesToViewModel(dto: OrderStateTypeDropdownListDto): IOrderSchedulingStatusListViewModel {
        return { name: dto.name, id: dto.id };
    }

    getProductionStatusByOrderId(orderId: string): Observable<IProductionStatusDetailsViewModel | null> {
        return this.orderSchedulingClient.getProductionStatusByOrderId(orderId).pipe(
            map(res => ({
                cuttingsStatus: res.cuttingsStatus ? res.cuttingsStatus : 0,
                cncStatus: res.cncStatus ? res.cncStatus : 0,
                edgebandingStatus: res.edgebandingStatus ? res.cuttingsStatus : 0
            }))
        );
    }

    getCFCsForPrintByOrderId(orderId: string): Observable<ICfcInformationListViewModel[]> {
        return this.cfcClient.getCFCsForPrintByOrderId(orderId).pipe(
            map((dto: ConcreteFurnitureComponentInformationListDto[]): ICfcInformationListViewModel[] => {
                return dto.map((x: ConcreteFurnitureComponentInformationListDto): ICfcInformationListViewModel => ({
                    workingNumber: x.workingNumber,
                    qrCode: {
                        containerName: x.image.containerName,
                        fileName: x.image.fileName
                    },
                    furnitureComponentCode: x.furnitureComponentCode,
                    topFoilCode: x.topFoilCode,
                    bottomFoilCode: x.bottomFoilCode,
                    leftFoilCode: x.leftFoilCode,
                    rightFoilCode: x.rightFoilCode,
                }));
            })
        );
    }

    startOptimistaion(order: IOptimisationModel): Observable<void> {
        const dto = new OrdersForOptimizationDto({
            orderIds: order.orderIds,
            shiftLengthHours: order.shiftLengthHours,
            shiftNumber: order.shiftNumber,
            sortingStrategyTypeId: order.sortingStrategyTypeId
        });

        return this.optimisationClient.startOrdersOptimization(dto);
    }

    getSortingStrategies(): Observable<IDropdownModel[]> {
        return this.sortingStrategyClient.getSortingStrategiesForDropdown().pipe(
            map((dto: SortingStrategyTypeListDto[]): IDropdownModel[] => {
                return dto.map((x: SortingStrategyTypeListDto): IDropdownModel => ({
                    id: x.id,
                    name: x.typeName
                }));
            })
        );
    }

    reserveOrFree(orderId: string, isReserved: boolean): Observable<void> {
        return this.reserveOrderClient.reserveOrFreeUpMaterialsForOrder(orderId, isReserved);
    }

    orderMaterial(id: string): Observable<number> {
        return this.orderMaterialClient.createRequiredMaterialByOrderId(id);
    }

}
