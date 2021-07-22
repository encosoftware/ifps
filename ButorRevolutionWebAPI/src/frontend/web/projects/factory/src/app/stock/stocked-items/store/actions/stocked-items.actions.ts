import { Action } from '@ngrx/store';
import { IStockedItemFilterListViewModel } from '../../models/stocked-items.model';

export enum ActionTypes {
    ChangeFilter = 'STOCKEDITEM:STOCKEDITEMLIST:CHANGE_FILTER',
    DeleteFilter = 'STOCKEDITEM:STOCKEDITEMLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IStockedItemFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type StockedItemsAction = ChangeFilter | DeleteFilter;
