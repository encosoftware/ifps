import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IFurnitureUnitFilterModel } from '../../models/furniture-unit.model';
import { IFUFeatureState } from '../state';

export const FurnitureUnitsSelector = createFeatureSelector<any, IFUFeatureState>('FurnitureUnits');

export const FurnitureUnitsFilters = createSelector(
    FurnitureUnitsSelector,
    (state): IFurnitureUnitFilterModel => state.FurnitureUnitsList.filters
);
