import { PagerModel } from '../../../shared/models/pager.model';

export interface ICuttingListViewModel {
    id: number;
    machine: string;
    material: string;
    orderId: string;
    workingNr: string;
    estimatedStartTime: Date;
    estimatedProcessTime: number;
    actualProcessTime: number;
    workerName: string[];
    isStarted: boolean;
    isShowedButton: boolean;
}

export interface ICuttingListFilterViewModel {
    machine?: string;
    material?: string;
    orderId?: string;
    workingNr?: string;
    estimatedStartTime?: Date;
    workerName?: string;
    pager: PagerModel;
}

