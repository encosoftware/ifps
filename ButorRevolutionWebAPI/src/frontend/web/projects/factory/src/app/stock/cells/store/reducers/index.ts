import { combineReducers, compose } from '@ngrx/store';
import { cellListReducers } from './cells.reducers';

export const cellsReducers = combineReducers({
    cellList: cellListReducers
});
