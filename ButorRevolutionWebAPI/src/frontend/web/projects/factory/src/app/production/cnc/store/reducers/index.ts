import { combineReducers, compose } from '@ngrx/store';
import { cncListReducers } from './cncs.reducers';

export const cncsReducers = combineReducers({
    cncList: cncListReducers
});
