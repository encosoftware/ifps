import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IOptimizationFeatureState } from '../state';
import { IOptimizationListFilterViewModel } from '../../models/optimization.model';


export const optimizationSelector = createFeatureSelector<any, IOptimizationFeatureState>('optimizations');

export const optimizationFilters = createSelector(
    optimizationSelector,
    (state): IOptimizationListFilterViewModel => state.optimizationList.filters
);
