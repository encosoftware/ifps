import { Action } from '@ngrx/store';
import { ICustomerOrdersFilterListViewModel } from '../../models/customerOrders';

export enum ActionTypes {
    ChangeFilter = 'ORDERSCUSTOMER:ORDERLIST:CHANGE_FILTER',
    DeleteFilter = 'ORDERSCUSTOMER:ORDERLIST:DELETE_FILTER',
}

export class ChangeFilter implements Action {
    readonly type = ActionTypes.ChangeFilter;

    constructor(public payload: ICustomerOrdersFilterListViewModel) { }
}


export class DeleteFilter implements Action {
    readonly type = ActionTypes.DeleteFilter;
}

export type OrdersCustomerAction = ChangeFilter | DeleteFilter;
