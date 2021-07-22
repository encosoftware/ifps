import { combineReducers, compose } from '@ngrx/store';
import { cargoListReducers } from './cargo.reducers';

export const cargoReducers = combineReducers({
    cargoList: cargoListReducers
});
