import { combineReducers, compose } from '@ngrx/store';
import { inspectionListReducers } from './inspection.reducer';

export const inspectionReducers = combineReducers({
    inspectionList: inspectionListReducers
});
