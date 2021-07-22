import { ICompanyFilterModel } from '../../models/company.model';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ICompaniesFeatureState } from '../state';

export const companiesSelector = createFeatureSelector<any, ICompaniesFeatureState>('companies');

export const companiesFilters = createSelector(
    companiesSelector,
    (state): ICompanyFilterModel => state.companyList.filters
);
