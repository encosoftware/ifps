import { PagerModel } from '../../../shared/models/pager.model';

export interface IOptimizationListViewModel {
    id: string;
    shiftNumber: number;
    shiftLength: number;
    startedAt: Date;
}

export interface IOptimizationListFilterViewModel {
    shiftNumber?: number;
    shiftLength?: number;
    startedAtFrom?: Date;
    startedAtTo?: Date;
    pager: PagerModel;
}
