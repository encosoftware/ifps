import { Action } from '@ngrx/store';
import { IOrdersFilterListViewModel } from '../../models/customerOrders';

export enum ActionTypes {
    ChangeFilter = 'ORDERSCUSTOMER:ORDERLIST:CHANGE_FILTER',
    DeleteFilter = 'ORDERSCUSTOMER:ORDERLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: IOrdersFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type OrdersCustomerAction = ChangeFilter | DeleteFilter;
