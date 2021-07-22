import { combineReducers, compose } from '@ngrx/store';
import { cargoListReducer } from './cargo-list.reducers';


export const cargoListReducers = combineReducers({
    cargoList: cargoListReducer
});
