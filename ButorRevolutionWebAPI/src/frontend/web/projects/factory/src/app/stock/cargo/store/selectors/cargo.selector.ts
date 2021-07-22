import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ICargoFilterListViewModel } from '../../models/cargo.model';
import { ICargoFeatureState } from '../state';


export const cargoSelector = createFeatureSelector<any, ICargoFeatureState>('cargo');

export const cargoFilters = createSelector(
    cargoSelector,
    (state): ICargoFilterListViewModel => state.cargoList.filters
);
