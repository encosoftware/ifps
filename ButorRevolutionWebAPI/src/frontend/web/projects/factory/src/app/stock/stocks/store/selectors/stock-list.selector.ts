import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IStorageFilterListViewModel } from '../../models/stocks.model';
import { IStorageFeatureState } from '../state';


export const storageSelector = createFeatureSelector<any, IStorageFeatureState>('stocks');

export const storageFilters = createSelector(
    storageSelector,
    (state): IStorageFilterListViewModel => state.stockList.filters
);
