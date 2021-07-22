import { Action } from '@ngrx/store';
import { ISupplyOrderFilterViewModel } from '../../models/supply-orders.model';

export enum ActionTypes {
    ChangeFilter = 'SUPPLY:SUPPLYORDERLIST:CHANGE_FILTER',
    DeleteFilter = 'SUPPLY:SUPPLYORDERLIST:DELETE_FILTER'
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ISupplyOrderFilterViewModel) { }
}

export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type SupplyOrdersAction = ChangeFilter
    | DeleteFilter;
