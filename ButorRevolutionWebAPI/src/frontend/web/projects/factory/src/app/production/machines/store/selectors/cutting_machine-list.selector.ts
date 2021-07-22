import { IMachinesFeatureState } from '../state';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ICuttingMachinesFilterViewModel } from '../../models/cutting_machines.model';

export const cuttingMachinesSelector = createFeatureSelector<any, IMachinesFeatureState>('cuttings');

export const cuttingMachinesFilters = createSelector(
    cuttingMachinesSelector,
    (state): ICuttingMachinesFilterViewModel => state.cuttingMachinesList.filters
);
