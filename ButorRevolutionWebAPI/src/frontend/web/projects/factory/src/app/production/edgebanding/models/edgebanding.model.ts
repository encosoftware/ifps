import { PagerModel } from '../../../shared/models/pager.model';

export interface IEdgebandingListViewModel {
    id: number;
    componentId: string;
    orderId: string;
    workingNr: string;
    estimatedStartTime: Date;
    estimatedProcessTime: number;
    actualProcessTime: number;
    workerName: string[];
    isStarted: boolean;
    isShowedButton: boolean;
}

export interface IEdgebandingListFilterViewModel {
    componentId?: string;
    orderId?: string;
    workingNr?: string;
    estimatedStartTime?: Date;
    workerName?: string;
    pager: PagerModel;
}
