import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ISupplyOrderFeatureState } from '../state';
import { ISupplyOrderFilterViewModel } from '../../models/supply-orders.model';

export const supplyOrdersSelector = createFeatureSelector<any, ISupplyOrderFeatureState>('supplyOrders');

export const supplyOrdersFilters = createSelector(
    supplyOrdersSelector,
    (state): ISupplyOrderFilterViewModel => state.supplyOrderList.filters
);
