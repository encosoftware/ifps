import { combineReducers } from '@ngrx/store';
import { materialPackagesListReducer } from './material-packages-list.reducer';

export const materialPackagesReducers = combineReducers({
    materialPackagesList: materialPackagesListReducer
});
