import { combineReducers, compose } from '@ngrx/store';
import { workstationsListReducer } from './workstations-list.reducer';

export const workstationsReducers = combineReducers({
    workstationsList: workstationsListReducer
});
