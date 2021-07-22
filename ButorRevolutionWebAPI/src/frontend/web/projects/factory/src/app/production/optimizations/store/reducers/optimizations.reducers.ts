
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IOptimizationListFilterViewModel } from '../../models/optimization.model';
import { OptimizationsAction, ActionTypes } from '../actions/optimizations.actions';

const initialState: IOptimizationListFilterViewModel = {
    shiftNumber: undefined,
    shiftLength: undefined,
    startedAtFrom: undefined,
    startedAtTo: undefined,
    pager: new PagerModel()
};

function optimizationListFilterReducer(
    state: IOptimizationListFilterViewModel = initialState,
    action: OptimizationsAction
): IOptimizationListFilterViewModel {

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

export const optimizationListReducers = combineReducers({
    filters: optimizationListFilterReducer
});
