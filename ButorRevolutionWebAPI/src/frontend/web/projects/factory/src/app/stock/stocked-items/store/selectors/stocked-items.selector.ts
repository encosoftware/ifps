import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IStockedItemFilterListViewModel } from '../../models/stocked-items.model';
import { IStockedItemFeatureState } from '../state';


export const stockedItemSelector = createFeatureSelector<any, IStockedItemFeatureState>('stockedItems');

export const stockedItemFilters = createSelector(
    stockedItemSelector,
    (state): IStockedItemFilterListViewModel => state.stockedItemList.filters
);
