import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IWFUFilterModel } from '../../models/wfurnitureunits.model';
import { IWFUFeatureState } from '../state';

export const wFurnitereUnitsSelector = createFeatureSelector<any, IWFUFeatureState>('wFurnitereUnits');

export const wFurnitereUnitsFilters = createSelector(
    wFurnitereUnitsSelector,
    (state): IWFUFilterModel => state.wFurnitereUnitsList.filters
);
