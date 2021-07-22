import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICameraFilterModel } from '../../models/cameras.model';
import { CamerasAction, ActionTypes } from '../actions/cameras-list.actions';

const initialState: ICameraFilterModel = {
    name: '',
    ipAddress: undefined,
    type: undefined,
    pager: new PagerModel()
};

function camerasListFilterReducer(
    state: ICameraFilterModel = initialState,
    action: CamerasAction
): ICameraFilterModel {
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

export const camerasListReducer = combineReducers({
    filters: camerasListFilterReducer
});
