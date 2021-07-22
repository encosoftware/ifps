import { IOrdersFilterListViewModel } from '../../models/orders';
import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IOrdersFeatureState } from '../state';


export const ordersSelector = createFeatureSelector<any, IOrdersFeatureState>('orders');

export const orderFilters = createSelector(
    ordersSelector,
    (state): IOrdersFilterListViewModel => state.orderList.filters
);
