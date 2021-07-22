import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IInspectionFilterListViewModel } from '../../models/inspection.model';
import { IInspectionFeatureState } from '../state';


export const inspectionSelector = createFeatureSelector<any, IInspectionFeatureState>('inspection');

export const inspectionFilters = createSelector(
    inspectionSelector,
    (state): IInspectionFilterListViewModel => state.inspectionList.filters
);
