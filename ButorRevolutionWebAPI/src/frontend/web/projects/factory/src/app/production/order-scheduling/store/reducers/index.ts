import { combineReducers, compose } from '@ngrx/store';
import { orderSchedulingListReducers } from './order-schedulings.reducers';

export const orderSchedulingsReducers = combineReducers({
    orderSchedulingList: orderSchedulingListReducers
});
