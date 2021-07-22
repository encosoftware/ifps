import { createSelector, createFeatureSelector } from '@ngrx/store';
import { IGeneralExpensesFeatureState } from '../state';
import { IGeneralExpensesListFilterViewModel } from '../../models/general-expenses.model';


export const generalExpensesSelector = createFeatureSelector<any, IGeneralExpensesFeatureState>('generalExpenses');

export const generalExpensesFilters = createSelector(
    generalExpensesSelector,
    (state): IGeneralExpensesListFilterViewModel => state.generalExpensesList.filters
);
