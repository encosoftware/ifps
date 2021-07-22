import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IVenueFilterListViewModel } from '../../models/venues.model';
import { IVenuesFeatureState } from '../state';


export const venuesSelector = createFeatureSelector<any, IVenuesFeatureState>('venues');

export const venueFilters = createSelector(
    venuesSelector,
    (state): IVenueFilterListViewModel => state.venueList.filters
);
