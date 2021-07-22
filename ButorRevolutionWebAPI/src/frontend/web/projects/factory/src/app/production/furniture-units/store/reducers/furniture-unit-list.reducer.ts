import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IFurnitureUnitFilterModel } from '../../models/furniture-unit.model';
import { FUAction, ActionTypes } from '../actions/furniture-units-list.actions';

const initialState: IFurnitureUnitFilterModel = {
    description: '',
    code: '',
    isUploaded: undefined,
    pager: new PagerModel()
};

function FurnitureUnitsListFilterReducer(
    state: IFurnitureUnitFilterModel = initialState,
    action: FUAction
): IFurnitureUnitFilterModel {
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
        default: {
            return state;
        }
    }
}

export const FurnitureUnitsListReducer = combineReducers({
    filters: FurnitureUnitsListFilterReducer
});
