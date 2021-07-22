import { IAppliancesFilterModel } from '../../models/appliences.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { ActionTypes, AppliancesAction } from '../actions/appliance-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: IAppliancesFilterModel = {
    brand: '',
    categoryId: undefined,
    category: '',
    code: '',
    description: '',
    hanaCode: '',
    pager: new PagerModel()
};

function applianceListFilterReducer(
    state: IAppliancesFilterModel = initialState,
    action: AppliancesAction
): IAppliancesFilterModel {
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

export const applianceListReducer = combineReducers({
    filters: applianceListFilterReducer
});
