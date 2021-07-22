import { combineReducers, compose } from '@ngrx/store';
import { edgebandingListReducers } from './edgebanding.reducers';

export const edgebandingsReducers = combineReducers({
    edgebandingList: edgebandingListReducers
});
