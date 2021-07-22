import { combineReducers } from '@ngrx/store';
import { wFurnitereUnitsListReducer } from './wfurnitureunit-list.reducer';

export const wFurnitereUnitsReducers = combineReducers({
    wFurnitereUnitsList: wFurnitereUnitsListReducer
});
