import { IFoilsFilterModel } from '../../models/foils.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { FoilsAction, ActionTypes } from '../actions/foil-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: IFoilsFilterModel = {
    code: '',
    description: '',
    pager: new PagerModel()
};

function foilListFilterReducer(
    state: IFoilsFilterModel = initialState,
    action: FoilsAction
): IFoilsFilterModel {
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

export const foilListReducer = combineReducers({
    filters: foilListFilterReducer
});
