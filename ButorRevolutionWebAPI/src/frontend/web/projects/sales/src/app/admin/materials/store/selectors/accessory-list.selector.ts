import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IMaterialsFeatureState } from '../state';
import { IAccessoryFilterModel } from '../../models/accessories.model';

export const accessorySelector = createFeatureSelector<any, IMaterialsFeatureState>('accessories');

export const accessoryFilter = createSelector(
    accessorySelector,
    (state): IAccessoryFilterModel => state.accessoryList.filters
);
