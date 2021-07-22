import { combineReducers } from '@ngrx/store';
import { camerasListReducer } from './cameras-list.reducer';

export const camerasReducers = combineReducers({
    camerasList: camerasListReducer
});
