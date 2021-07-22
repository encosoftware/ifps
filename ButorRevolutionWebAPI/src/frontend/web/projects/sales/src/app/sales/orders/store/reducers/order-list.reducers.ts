
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IOrdersFilterListViewModel } from '../../models/orders';
import { OrdersAction, ActionTypes } from '../actions/order-list.actions';

const initialState: IOrdersFilterListViewModel = {
    orderId: '',
    workingNr: '',
    currentStatus: undefined,
    statusDeadline: undefined,
    responsible: '',
    customer: '',
    sales: '',
    deadline: undefined,
    created: undefined,
    pager: new PagerModel()
};

function orderListFilterReducer(
    state: IOrdersFilterListViewModel = initialState,
    action: OrdersAction
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

export const ordersListReducers = combineReducers({
    filters: orderListFilterReducer
});
