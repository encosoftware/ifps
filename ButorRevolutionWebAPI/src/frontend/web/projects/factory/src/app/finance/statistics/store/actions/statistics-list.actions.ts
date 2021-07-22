import { Action } from '@ngrx/store';
import { IStatisticsFilterModel } from '../../models/statistics.model';

export enum ActionTypes {
    ChangeFilter = 'STATISTICS:STATISTICSLIST:CHANGE_FILTER',
    DeleteFilter = 'STATISTICS:STATISTICSLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IStatisticsFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type StatisticsAction = ChangeFilter
    | DeleteFilter;
