import { Action } from '@ngrx/store';
import { IOrdersFilterListViewModel } from '../../models/orders';

export enum ActionTypes {
    ChangeFilter = 'ORDERS:ORDERLIST:CHANGE_FILTER',
    DeleteFilter = 'ORDERS:ORDERLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IOrdersFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type OrdersAction = ChangeFilter | DeleteFilter;
