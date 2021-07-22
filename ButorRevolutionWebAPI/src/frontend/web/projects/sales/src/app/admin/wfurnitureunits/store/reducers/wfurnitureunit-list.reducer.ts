import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IWFUFilterModel } from '../../models/wfurnitureunits.model';
import { WFUAction, ActionTypes } from '../actions/wfurnitureunits-list.actions';

const initialState: IWFUFilterModel = {
    description: '',
    code: '',
    pager: new PagerModel()
};

function wFurnitereUnitsListFilterReducer(
    state: IWFUFilterModel = initialState,
    action: WFUAction
): IWFUFilterModel {
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

export const wFurnitereUnitsListReducer = combineReducers({
    filters: wFurnitereUnitsListFilterReducer
});
