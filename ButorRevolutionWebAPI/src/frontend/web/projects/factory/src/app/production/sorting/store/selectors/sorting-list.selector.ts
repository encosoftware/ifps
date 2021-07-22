import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ISortingFeatureState } from '../states';
import { ISortingListFilterViewModel } from '../../models/sorting.model';

export const sortingSelector = createFeatureSelector<any, ISortingFeatureState>('sorting');

export const sortingFilters = createSelector(
    sortingSelector,
    (state): ISortingListFilterViewModel => state.sortingList.filters
);
