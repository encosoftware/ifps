
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICncListFilterViewModel } from '../../models/cnc.model';
import { CncsAction, ActionTypes } from '../actions/cncs.actions';

const initialState: ICncListFilterViewModel = {
    componentId: '',
    estimatedStartTime: undefined,
    material: '',
    orderId: '',
    workerName: '',
    workingNr: '',
    pager: new PagerModel()
};

function cncListFilterReducer(
    state: ICncListFilterViewModel = initialState,
    action: CncsAction
): ICncListFilterViewModel {

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

export const cncListReducers = combineReducers({
    filters: cncListFilterReducer
});
