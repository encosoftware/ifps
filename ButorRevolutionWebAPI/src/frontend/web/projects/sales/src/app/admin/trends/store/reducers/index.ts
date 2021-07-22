import { ActionReducer, combineReducers, compose } from '@ngrx/store';
import { trendsListReducer } from './trend-list.reducers';

export const trendsReducers = combineReducers({
    trendList: trendsListReducer
});


