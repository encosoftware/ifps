import { combineReducers, compose } from '@ngrx/store';
import { ordersListCustomerReducers } from './order-list.reducers';

export const ordersCustomerReducers = combineReducers({
    orderCustomerList: ordersListCustomerReducers
});
