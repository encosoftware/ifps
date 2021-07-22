import { IAccessoryFilterModel } from '../../models/accessories.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { AccessoriesAction, ActionTypes } from '../actions/accessory-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: IAccessoryFilterModel = {
    code: '',
    categoryId: undefined,
    description: '',
    optMount: undefined,
    structurallyOptional: undefined,
    pager: new PagerModel()
};

function accessoryListFilterReducer(
    state: IAccessoryFilterModel = initialState,
    action: AccessoriesAction
): IAccessoryFilterModel {
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

export const accessoryListReducer = combineReducers({
    filters: accessoryListFilterReducer
});
