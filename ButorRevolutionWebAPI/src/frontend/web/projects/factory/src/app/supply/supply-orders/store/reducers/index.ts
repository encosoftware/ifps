import { combineReducers, compose } from '@ngrx/store';
import { supplyOrderListReducer } from './supply-order-list.reducers';

export const supplyOrdersReducers = combineReducers({
    supplyOrderList: supplyOrderListReducer
});
