import { Action } from '@ngrx/store';
import { IStorageFilterListViewModel } from '../../models/stocks.model';


export enum ActionTypes {
    ChangeFilter = 'STOCKS:STOCKLIST:CHANGE_FILTER',
    DeleteFilter = 'STOCKS:STOCKLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IStorageFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type StorageAction = ChangeFilter | DeleteFilter;
