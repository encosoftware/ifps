import { combineReducers } from '@ngrx/store';
import { FurnitureUnitsListReducer } from './furniture-unit-list.reducer';

export const FurnitureUnitsReducers = combineReducers({
    FurnitureUnitsList: FurnitureUnitsListReducer
});
