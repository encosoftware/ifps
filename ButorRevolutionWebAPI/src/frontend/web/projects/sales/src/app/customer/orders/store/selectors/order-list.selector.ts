import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ICustomerOrdersFilterListViewModel } from '../../models/customerOrders';
import { IOrdersCustomerFeatureState } from '../state';


export const ordersCustomerSelector = createFeatureSelector<any, IOrdersCustomerFeatureState>('ordersCustomer');

export const orderCustomerFilters = createSelector(
    ordersCustomerSelector,
    (state): ICustomerOrdersFilterListViewModel => state.orderCustomerList.filtersCustomer
);
