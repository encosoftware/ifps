import { combineReducers, compose } from '@ngrx/store';
import { assemblyListReducers } from './assembly.reducers';

export const assemblysReducers = combineReducers({
    assemblyList: assemblyListReducers
});
