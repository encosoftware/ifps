import { IMachinesFeatureState } from '../state';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IEdgingMachinesFilterViewModel } from '../../models/edging_machines.model';

export const edgingMachinesSelector = createFeatureSelector<any, IMachinesFeatureState>('edgings');

export const edgingMachinesFilters = createSelector(
    edgingMachinesSelector,
    (state): IEdgingMachinesFilterViewModel => state.edgingMachinesList.filters
);
