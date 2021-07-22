import { Action } from '@ngrx/store';
import { IStatisticsFilterViewModel } from '../../models/statistics';

export enum ActionTypes {
    ChangeFilter = 'STATICS:STATICSLIST:CHANGE_FILTER',
    DeleteFilter = 'STATICS:STATICSLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IStatisticsFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type StatisticsAction = ChangeFilter | DeleteFilter;
