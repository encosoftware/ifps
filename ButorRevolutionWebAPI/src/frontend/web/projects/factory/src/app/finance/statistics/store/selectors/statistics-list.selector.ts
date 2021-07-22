import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IStatisticsFilterModel } from '../../models/statistics.model';
import { IStatisticsFeatureState } from '../state';

export const statisticsSelector = createFeatureSelector<any, IStatisticsFeatureState>('statistics');

export const statisticsFilters = createSelector(
    statisticsSelector,
    (state): IStatisticsFilterModel => state.statisticsList.filters
);
