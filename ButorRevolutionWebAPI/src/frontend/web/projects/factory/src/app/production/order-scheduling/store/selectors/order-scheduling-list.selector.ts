import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IOrderSchedulingFeatureState } from '../state';
import { IOrderSchedulingListFilterViewModel } from '../../models/order-scheduling.model';


export const orderSchedulingSelector = createFeatureSelector<any, IOrderSchedulingFeatureState>('orderSchedulings');

export const orderSchedulingFilters = createSelector(
    orderSchedulingSelector,
    (state): IOrderSchedulingListFilterViewModel => state.orderSchedulingList.filters
);
