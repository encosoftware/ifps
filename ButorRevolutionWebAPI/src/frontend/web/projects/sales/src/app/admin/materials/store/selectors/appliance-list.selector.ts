import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IMaterialsFeatureState } from '../state';
import { IAppliancesFilterModel } from '../../models/appliences.model';

export const applianceSelector = createFeatureSelector<any, IMaterialsFeatureState>('appliances');

export const appliancesFilters = createSelector(
    applianceSelector,
    (state): IAppliancesFilterModel => state.applianceList.filters
);
