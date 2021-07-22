import { Action } from '@ngrx/store';
import { IWFUFilterModel } from '../../models/wfurnitureunits.model';

export enum ActionTypes {
    ChangeFilter = 'WFUS:WFULIST:CHANGE_FILTER',
    DeleteFilter = 'WFUS:WFULIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IWFUFilterModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type WFUAction = ChangeFilter
    | DeleteFilter;
