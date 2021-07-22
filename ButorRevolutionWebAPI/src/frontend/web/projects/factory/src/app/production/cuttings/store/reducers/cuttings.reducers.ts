
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICuttingListFilterViewModel } from '../../models/cuttings.model';
import { CuttingsAction, ActionTypes } from '../actions/cuttings.actions';

const initialState: ICuttingListFilterViewModel = {
    workerName: '',
    estimatedStartTime: undefined,
    machine: '',
    material: '',
    orderId: '',
    workingNr: '',
    pager: new PagerModel()
};

function cuttingListFilterReducer(
    state: ICuttingListFilterViewModel = initialState,
    action: CuttingsAction
): ICuttingListFilterViewModel {

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

export const cuttingListReducers = combineReducers({
    filters: cuttingListFilterReducer
});
