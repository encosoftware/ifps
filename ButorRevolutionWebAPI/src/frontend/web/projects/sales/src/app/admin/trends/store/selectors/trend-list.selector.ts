import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ITrendFilterModel } from '../../models/trends.models';
import { ITrendsFeatureState } from '../state';

export const trendsSelector = createFeatureSelector<any, ITrendsFeatureState>('trends');

export const trendsFilters = createSelector(
    trendsSelector,
    (state): ITrendFilterModel => state.trendList.filters
);
