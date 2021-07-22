import { IWorktopFilterModel } from '../../models/worktops.model';
import { WorktopsAction, ActionTypes } from '../actions/worktop-list.actions';
import { combineReducers } from '@ngrx/store';
import { PagerModel } from '../../../../shared/models/pager.model';

const initialState: IWorktopFilterModel = {
    category: '',
    categoryId: undefined,
    code: '',
    description: '',
    pager: new PagerModel()
};

function worktopListFilterReducer(
    state: IWorktopFilterModel = initialState,
    action: WorktopsAction
): IWorktopFilterModel {
    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };
        case ActionTypes.DeleteFilter:
            return {
                ...initialState
            };
    }

    return state;
}

export const worktopListReducer = combineReducers({
    filters: worktopListFilterReducer
});
