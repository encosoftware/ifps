import { combineReducers, compose } from '@ngrx/store';
import { stockedItemListReducers } from './stocked-items.reducers';

export const stockeditemReducers = combineReducers({
    stockedItemList: stockedItemListReducers
});
