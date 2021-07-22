import { combineReducers } from '@ngrx/store';
import { statisticsListReducers } from './statistics.reducers';

export const statisticsReducers = combineReducers({
    statisticsList: statisticsListReducers
});
