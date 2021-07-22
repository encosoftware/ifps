import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IPackingFeatureState } from '../states';
import { IPackingListFilterViewModel } from '../../models/packing.model';

export const packingSelector = createFeatureSelector<any, IPackingFeatureState>('packing');

export const packingFilters = createSelector(
    packingSelector,
    (state): IPackingListFilterViewModel => state.packingList.filters
);
