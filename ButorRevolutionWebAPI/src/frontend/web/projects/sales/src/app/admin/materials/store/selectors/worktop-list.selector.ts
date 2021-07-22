import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IMaterialsFeatureState } from '../state';
import { IWorktopFilterModel } from '../../models/worktops.model';

export const worktopsSelector = createFeatureSelector<any, IMaterialsFeatureState>('worktops');

export const worktopsFilters = createSelector(
    worktopsSelector,
    (state): IWorktopFilterModel => state.worktopList.filters
);
