
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICargoFilterListViewModel } from '../../models/cargo.model';
import { CargoAction, ActionTypes } from '../actions/cargo.actions';

const initialState: ICargoFilterListViewModel = {
    cargoId: '',
    bookedBy: '',
    arrivedOn: undefined,
    status: undefined,
    supplier: undefined,
    pager: new PagerModel()
};

function cargoListFilterReducer(
    state: ICargoFilterListViewModel = initialState,
    action: CargoAction
): ICargoFilterListViewModel {

    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };
        case ActionTypes.DeleteFilter:
            return { ...initialState };
    }

    return state;
}

export const cargoListReducers = combineReducers({
    filters: cargoListFilterReducer
});
