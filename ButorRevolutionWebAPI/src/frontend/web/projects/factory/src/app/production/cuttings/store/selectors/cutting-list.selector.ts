import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ICuttingFeatureState } from '../state';
import { ICuttingListFilterViewModel } from '../../models/cuttings.model';


export const cuttingSelector = createFeatureSelector<any, ICuttingFeatureState>('cuttings');

export const cuttingFilters = createSelector(
    cuttingSelector,
    (state): ICuttingListFilterViewModel => state.cuttingList.filters
);
