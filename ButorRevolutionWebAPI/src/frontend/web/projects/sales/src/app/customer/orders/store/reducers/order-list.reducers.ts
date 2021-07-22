
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICustomerOrdersFilterListViewModel } from '../../models/customerOrders';
import { OrdersCustomerAction, ActionTypes } from '../actions/order-list.action';

const initialState: ICustomerOrdersFilterListViewModel = {
    id: undefined,
    orderId: undefined,
    workingNumber: undefined,
    currentStatus: undefined,
    statusDeadline: undefined,
    responsibleName: undefined,
    customerName: undefined,
    salesName: undefined,
    deadline: undefined,
    createdOnFrom: undefined,
    pager: new PagerModel()
};

function orderListFilterReducer(
    state: ICustomerOrdersFilterListViewModel = initialState,
    action: OrdersCustomerAction
): ICustomerOrdersFilterListViewModel {

    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };
        case ActionTypes.DeleteFilter:
            return { ...initialState };
    }

    return state;
}

export const ordersListCustomerReducers = combineReducers({
    filtersCustomer: orderListFilterReducer
});
