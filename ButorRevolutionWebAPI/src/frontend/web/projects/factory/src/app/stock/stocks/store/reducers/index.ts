import { combineReducers, compose } from '@ngrx/store';
import { stockListReducers } from './stocks.reducers';

export const stockReducers = combineReducers({
    stockList: stockListReducers
});
