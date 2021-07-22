import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IMaterialsFeatureState } from '../state';
import { IDecorboardFilterModel } from '../../models/decorboards.model';

export const decorboardSelector = createFeatureSelector<any, IMaterialsFeatureState>('decorboards');

export const decorboardFilters = createSelector(
    decorboardSelector,
    (state): IDecorboardFilterModel => state.decorboardList.filters
);
