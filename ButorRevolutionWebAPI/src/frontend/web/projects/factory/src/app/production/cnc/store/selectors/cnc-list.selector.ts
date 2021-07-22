import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ICncListFilterViewModel } from '../../models/cnc.model';
import { ICncFeatureState } from '../state';


export const cncSelector = createFeatureSelector<any, ICncFeatureState>('cncs');

export const cncFilters = createSelector(
    cncSelector,
    (state): ICncListFilterViewModel => state.cncList.filters
);
