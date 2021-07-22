import { ICargoFilterModel } from '../../models/cargo.model';
import { PagerModel } from '../../../../../../../factory/src/app/shared/models/pager.model';
import { CargoListAction, ActionTypes } from '../actions/cargo-list.actions';
import { combineReducers } from '@ngrx/store';

const initialState: ICargoFilterModel = {
    bookedBy: '',
    cargoId: '',
    created: undefined,
    status: undefined,
    supplier: undefined,
    pager: new PagerModel()
};

function cargoListFilterReducer(
    state: ICargoFilterModel = initialState,
    action: CargoListAction
): ICargoFilterModel {
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

export const cargoListReducer = combineReducers({
    filters: cargoListFilterReducer
});
