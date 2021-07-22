import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IMaterialsFeatureState } from '../state';
import { IFoilsFilterModel } from '../../models/foils.model';

export const foilsSelector = createFeatureSelector<any, IMaterialsFeatureState>('foils');

export const foilsFilters = createSelector(
    foilsSelector,
    (state): IFoilsFilterModel => state.foilList.filters
);
