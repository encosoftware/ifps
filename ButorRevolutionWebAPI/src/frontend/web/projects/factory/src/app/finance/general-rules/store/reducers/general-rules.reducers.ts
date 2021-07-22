
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IGeneralRulesListFilterViewModel } from '../../models/general-rules.model';
import { GeneralRulesAction, ActionTypes } from '../actions/general-rules.actions';

const initialState: IGeneralRulesListFilterViewModel = {
    amount: undefined,
    name: '',
    startDate: undefined,
    frequencyFrom: undefined,
    frequencyTo: undefined,
    frequencyTypeId: undefined,
    pager: new PagerModel()
};

function generalRulesListFilterReducer(
    state: IGeneralRulesListFilterViewModel = initialState,
    action: GeneralRulesAction
): IGeneralRulesListFilterViewModel {

    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };
        case ActionTypes.DeleteFilter:
            return { ...initialState };
    }

    return state;
}

export const generalRulesListReducers = combineReducers({
    filters: generalRulesListFilterReducer
});
