import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ICamerasFeatureState } from '../state';
import { ICameraFilterModel } from '../../models/cameras.model';

export const camerasSelector = createFeatureSelector<any, ICamerasFeatureState>('cameras');

export const camerasFilters = createSelector(
    camerasSelector,
    (state): ICameraFilterModel => state.camerasList.filters
);
