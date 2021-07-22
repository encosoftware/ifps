import { IDecorboardFilterModel } from '../../models/decorboards.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { DecoboardsAction, ActionTypes } from '../actions/decorboard-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: IDecorboardFilterModel = {
    category: '',
    categoryId: undefined,
    code: '',
    description: '',
    pager: new PagerModel()
};

function decorboardListFilterReducer(
    state: IDecorboardFilterModel = initialState,
    action: DecoboardsAction
): IDecorboardFilterModel {
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

export const decorboardListReducer = combineReducers({
    filters: decorboardListFilterReducer
});
