import { combineReducers, compose } from '@ngrx/store';
import { generalExpensesListReducers } from './general-expenses.reducers';

export const generalExpensesReducers = combineReducers({
    generalExpensesList: generalExpensesListReducers
});
