import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IOrdersFilterListViewModel } from '../../models/customerOrders';
import { IOrdersCustomerFeatureState } from '../state';


export const ordersCustomerSelector = createFeatureSelector<any, IOrdersCustomerFeatureState>('ordersFinances');

export const orderCustomerFilters = createSelector(
    ordersCustomerSelector,
    (state): IOrdersFilterListViewModel => state.orderCustomerList.filtersCustomer
);
