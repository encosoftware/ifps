
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IOrdersFilterListViewModel } from '../../models/customerOrders';
import { OrdersCustomerAction, ActionTypes } from '../actions/order-list.action';

const initialState: IOrdersFilterListViewModel = {
    orderId: '',
    currentStatus: '',
    workingNr: '',
    // currentStatus?: OrderStateEnum | string;
    statusDeadline: undefined,
    amount: undefined,
    pager: new PagerModel(),
};

function orderListFilterReducer(
    state: IOrdersFilterListViewModel = initialState,
    action: OrdersCustomerAction
): IOrdersFilterListViewModel {

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
