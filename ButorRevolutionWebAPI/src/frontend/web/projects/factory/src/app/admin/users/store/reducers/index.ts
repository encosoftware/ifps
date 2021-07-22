import { combineReducers, ActionReducer, compose } from '@ngrx/store';
import { IUsersFeatureState } from '../state';
import { usersListReducers } from './user-list.reducers';

export const usersReducers = combineReducers({
    userList: usersListReducers
});
