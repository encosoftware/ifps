import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ICellFilterListViewModel } from '../../models/cells.model';
import { ICellFeatureState } from '../state';


export const cellSelector = createFeatureSelector<any, ICellFeatureState>('cells');

export const cellFilters = createSelector(
    cellSelector,
    (state): ICellFilterListViewModel => state.cellList.filters
);
