import { ActionReducer, combineReducers, compose } from '@ngrx/store';
import { ordersListCustomerReducers } from './order-list.reducers';
import { IOrdersCustomerFeatureState } from '../state';

export const ordersCustomerReducers = combineReducers({
    orderCustomerList: ordersListCustomerReducers
});
