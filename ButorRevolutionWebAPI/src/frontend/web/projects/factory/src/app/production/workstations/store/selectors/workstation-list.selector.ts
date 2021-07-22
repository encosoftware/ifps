import { IWorkstationsFilterModel } from '../../models/workstations.model';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IWorkstationsFeatureState } from '../state';

export const workstationsSelector = createFeatureSelector<any, IWorkstationsFeatureState>('workstations');

export const workstationsFilters = createSelector(
    workstationsSelector,
    (state): IWorkstationsFilterModel => state.workstationsList.filters
);
