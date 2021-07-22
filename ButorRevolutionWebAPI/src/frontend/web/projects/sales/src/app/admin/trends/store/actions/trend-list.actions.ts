import { Action } from '@ngrx/store';
import { ITrendFilterModel } from '../../models/trends.models';

export enum ActionTypes {
    ChangeFilter = 'TRENDS:TRENDLIST:CHANGE_FILTER',
    DeleteFilter = 'TRENDS:TRENDLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ITrendFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type TrendsAction = ChangeFilter
    | DeleteFilter;
