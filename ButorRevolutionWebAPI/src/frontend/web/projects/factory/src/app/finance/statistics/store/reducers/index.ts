import { combineReducers } from '@ngrx/store';
import { statisticsListReducer } from './statistics-list.reducer';

export const statisticsReducers = combineReducers({
    statisticsList: statisticsListReducer
});
