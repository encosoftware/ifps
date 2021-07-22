import { PagerModel } from '../../../shared/models/pager.model';

export interface IOrderSchedulingListViewModel {
    selectable: boolean;
    isSelected?: boolean;
    orderId: string;
    orderName: string;
    workingNr: string;
    status: string;
    deadline: Date;
    showReserve?: boolean;
    showOrderMaterial?: boolean;
    showStatus: boolean;
    // estimatedProcessTime: number;
    // showTime: boolean;
}

export interface IOrderSchedulingListFilterViewModel {
    orderId?: string;
    workingNr?: string;
    statusId?: number;
    completion?: number;
    deadline?: Date;
    pager: PagerModel;
}

export interface IOrderSchedulingStatusListViewModel {
    name: string;
    id: number;
}

export interface IProductionStatusDetailsViewModel {
    orderId?: string;
    cuttingsStatus: number;
    cncStatus: number;
    edgebandingStatus: number;
}

export interface ICfcInformationListViewModel {
    qrCode: IImageDetailsModel;
    workingNumber: string;
    furnitureComponentCode: string;
    topFoilCode: string;
    bottomFoilCode: string;
    leftFoilCode: string;
    rightFoilCode: string;
}

export interface IOptimisationModel {
    orderIds: string[];
    shiftNumber: number;
    shiftLengthHours: number;
    sortingStrategyTypeId: number;
}

export interface IDropdownModel {
    id: number;
    name: string;
}

export interface IImageDetailsModel {
    containerName?: string;
    fileName?: string;
}