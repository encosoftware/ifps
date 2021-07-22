import { PagerModel } from '../../../shared/models/pager.model';

export interface ISortingListViewModel {
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

export interface ISortingListFilterViewModel {
    unitId?: string;
    orderId?: string;
    workingNr?: string;
    date?: Date;
    workerName?: string;
    pager: PagerModel;
}
