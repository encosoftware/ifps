import { combineReducers } from '@ngrx/store';
import { optimizationListReducers } from './optimizations.reducers';

export const optimizationsReducers = combineReducers({
    optimizationList: optimizationListReducers
});
