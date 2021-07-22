import { Action } from '@ngrx/store';
import { IOptimizationListFilterViewModel } from '../../models/optimization.model';

export enum ActionTypes {
    ChangeFilter = 'OPTIMIZATIONS:OPTIMIZATIONLIST:CHANGE_FILTER',
    DeleteFilter = 'OPTIMIZATIONS:OPTIMIZATIONLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IOptimizationListFilterViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type OptimizationsAction = ChangeFilter | DeleteFilter;
