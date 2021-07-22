import { combineReducers, ActionReducer, compose } from '@ngrx/store';
import { IOrdersFeatureState } from '../state';
import { ordersListReducers } from './order-list.reducers';

export const ordersReducers: ActionReducer<IOrdersFeatureState> = combineReducers({
    orderList: ordersListReducers
});
