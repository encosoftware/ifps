import { combineReducers, compose } from '@ngrx/store';
import { cuttingListReducers } from './cuttings.reducers';

export const cuttingsReducers = combineReducers({
    cuttingList: cuttingListReducers
});
