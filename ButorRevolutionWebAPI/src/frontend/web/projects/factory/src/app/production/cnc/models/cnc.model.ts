import { PagerModel } from '../../../shared/models/pager.model';

export interface ICncListViewModel {
    id: number;
    componentId: string;
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

export interface ICncListFilterViewModel {
    componentId?: string;
    material?: string;
    orderId?: string;
    workingNr?: string;
    estimatedStartTime?: Date;
    workerName?: string;
    pager: PagerModel;
}
