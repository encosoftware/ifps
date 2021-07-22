import { IMachinesFeatureState } from '../state';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ICncMachinesFilterViewModel } from '../../models/cnc_machines.model';

export const cncMachinesSelector = createFeatureSelector<any, IMachinesFeatureState>('cncs');

export const cncMachinesFilters = createSelector(
    cncMachinesSelector,
    (state): ICncMachinesFilterViewModel => state.cncMachinesList.filters
);
