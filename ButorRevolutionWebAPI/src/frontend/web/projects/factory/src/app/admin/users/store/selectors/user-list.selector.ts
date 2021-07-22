import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IUsersFilterViewModel } from '../../models/users.models';
import { IUsersFeatureState } from '../state';

export const usersSelector = createFeatureSelector<any, IUsersFeatureState>('users');

export const userFilters = createSelector(
    usersSelector,
    (state): IUsersFilterViewModel => state.userList.filters
);

