import { ActionReducer, combineReducers, compose } from '@ngrx/store';
import { companiesListReducer } from './company-list.reducers';
import { ICompaniesFeatureState } from '../state/index';

export const companiesReducers = combineReducers({
    companyList: companiesListReducer
});


