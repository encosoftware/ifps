import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IAssemblyFeatureState } from '../state';
import { IAssemblyListFilterViewModel } from '../../models/assembly.model';

export const assemblySelector = createFeatureSelector<any, IAssemblyFeatureState>('assemblies');

export const assemblyFilters = createSelector(
    assemblySelector,
    (state): IAssemblyListFilterViewModel => state.assemblyList.filters
);
