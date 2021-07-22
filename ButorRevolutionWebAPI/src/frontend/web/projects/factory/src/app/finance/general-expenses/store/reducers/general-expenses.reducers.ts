
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IGeneralExpensesListFilterViewModel } from '../../models/general-expenses.model';
import { ActionTypes, GeneralExpensesAction } from '../actions/general-expenses.actions';

const initialState: IGeneralExpensesListFilterViewModel = {
    amount: undefined,
    name: '',
    paymentDate: undefined,
    pager: new PagerModel()
};

function generalExpensesListFilterReducer(
    state: IGeneralExpensesListFilterViewModel = initialState,
    action: GeneralExpensesAction
): IGeneralExpensesListFilterViewModel {

    switch (action.type) {
        case ActionTypes.ChangeFilter:
            if (+action.payload.amount === 0) {
                action.payload.amount = undefined;
            }
            return {
                ...state,
                ...action.payload
            };
        case ActionTypes.DeleteFilter:
            return { ...initialState };
    }

    return state;
}

export const generalExpensesListReducers = combineReducers({
    filters: generalExpensesListFilterReducer
});
