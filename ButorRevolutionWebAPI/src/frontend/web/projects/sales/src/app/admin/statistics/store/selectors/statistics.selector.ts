import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IStatisticsFilterViewModel } from '../../models/statistics';
import { IStatisticsFeatureState } from '../state';

export const statisticsSelector =
 createFeatureSelector<any, IStatisticsFeatureState>('statistics');

export const statisticsFilters = createSelector(
    statisticsSelector,
    (state): IStatisticsFilterViewModel => state.statisticsList.filters
);
