import { IPackingListFilterViewModel } from '../../models/packing.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { PackingAction, ActionTypes } from '../actions/packing.actions';

const initialState: IPackingListFilterViewModel = {
    orderId: '',
    unitId: '',
    workingNr: '',
    date: undefined,
    workerName: '',
    pager: new PagerModel()
};

function packingListFilterReducer(
    state: IPackingListFilterViewModel = initialState,
    action: PackingAction
): IPackingListFilterViewModel {

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

export const packingListReducers = combineReducers({
    filters: packingListFilterReducer
});
