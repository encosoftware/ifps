import { combineReducers, ActionReducer, compose } from '@ngrx/store';
import { usersListReducers } from './user-list.reducers';

export const usersReducers = combineReducers({
    userList: usersListReducers
});
