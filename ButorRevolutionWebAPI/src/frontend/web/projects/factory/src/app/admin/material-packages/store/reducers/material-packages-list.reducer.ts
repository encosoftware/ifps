import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IMaterialPackageFilterModel } from '../../models/material-packages.model';
import { MaterialPackageAction, ActionTypes } from '../actions/material-packages-list.actions';

const initialState: IMaterialPackageFilterModel = {
    code: '',
    description: '',
    supplierName: '',
    size: undefined,
    pager: new PagerModel()
};

function materialPackagesListFilterReducer(
    state: IMaterialPackageFilterModel = initialState,
    action: MaterialPackageAction
): IMaterialPackageFilterModel {
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

export const materialPackagesListReducer = combineReducers({
    filters: materialPackagesListFilterReducer
});
