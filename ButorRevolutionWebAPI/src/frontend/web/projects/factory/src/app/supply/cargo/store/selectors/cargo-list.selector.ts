import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ICargoListFeatureState } from '../state';
import { ICargoFilterModel } from '../../models/cargo.model';

export const cargSelector = createFeatureSelector<any, ICargoListFeatureState>('cargos');

export const cargoListFilters = createSelector(
    cargSelector,
    (state): ICargoFilterModel => state.cargoList.filters
);
