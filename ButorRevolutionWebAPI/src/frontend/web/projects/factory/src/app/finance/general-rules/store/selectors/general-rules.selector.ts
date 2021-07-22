import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IGeneralRulesFeatureState } from '../state';
import { IGeneralRulesListFilterViewModel } from '../../models/general-rules.model';

export const generalRulesSelector = createFeatureSelector<any, IGeneralRulesFeatureState>('generalRules');

export const generalRulesFilters = createSelector(
    generalRulesSelector,
    (state): IGeneralRulesListFilterViewModel => state.generalRulesList.filters
);
