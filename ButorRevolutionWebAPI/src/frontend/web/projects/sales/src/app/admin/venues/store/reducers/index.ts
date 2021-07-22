import { combineReducers, ActionReducer, compose } from '@ngrx/store';
import { venuesListReducers } from './venues.reducers';
import { IVenuesFeatureState } from '../state';

export const venuesReducers = combineReducers({
    venueList: venuesListReducers
});
