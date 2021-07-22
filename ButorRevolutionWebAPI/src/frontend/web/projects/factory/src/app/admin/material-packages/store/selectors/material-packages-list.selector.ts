import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IMaterialPackageFilterModel } from '../../models/material-packages.model';
import { IMaterialPackagesFeatureState } from '../state';


export const materialPackagesSelector = createFeatureSelector<any, IMaterialPackagesFeatureState>('materialPackages');

export const materialPackageFilters = createSelector(
    materialPackagesSelector,
    (state): IMaterialPackageFilterModel => state.materialPackagesList.filters
);
