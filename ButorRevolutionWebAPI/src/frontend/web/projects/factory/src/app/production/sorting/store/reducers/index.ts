import { combineReducers } from '@ngrx/store';
import { sortingListReducers } from './sorting.reducers';

export const sortingsReducers = combineReducers({
    sortingList: sortingListReducers
});
