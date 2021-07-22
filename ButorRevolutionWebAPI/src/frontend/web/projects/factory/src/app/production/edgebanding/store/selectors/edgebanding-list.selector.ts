import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IEdgebandingFeatureState } from '../state';
import { IEdgebandingListFilterViewModel } from '../../models/edgebanding.model';


export const edgebandingSelector = createFeatureSelector<any, IEdgebandingFeatureState>('edgebandings');

export const edgebandingFilters = createSelector(
    edgebandingSelector,
    (state): IEdgebandingListFilterViewModel => state.edgebandingList.filters
);
