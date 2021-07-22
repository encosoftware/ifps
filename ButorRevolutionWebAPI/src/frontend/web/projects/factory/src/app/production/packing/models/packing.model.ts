import { PagerModel } from '../../../shared/models/pager.model';

export interface IPackingListViewModel {
    id: number;
    unitId: string;
    orderId: string;
    workingNr: string;
    estimatedStartTime: Date;
    estimatedProcessTime: number;
    actualProcessTime: number;
    workerName: string[];
    isStarted: boolean;
    isShowedButton: boolean;
}

export interface IPackingListFilterViewModel {
    unitId?: string;
    orderId?: string;
    workingNr?: string;
    date?: Date;
    workerName?: string;
    pager: PagerModel;
}
