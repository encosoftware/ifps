
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IEdgebandingListFilterViewModel } from '../../models/edgebanding.model';
import { EdgebandingsAction, ActionTypes } from '../actions/edgebandings.actions';

const initialState: IEdgebandingListFilterViewModel = {
    componentId: '',
    estimatedStartTime: undefined,
    orderId: '',
    workerName: '',
    workingNr: '',
    pager: new PagerModel()
};

function edgebandingListFilterReducer(
    state: IEdgebandingListFilterViewModel = initialState,
    action: EdgebandingsAction
): IEdgebandingListFilterViewModel {

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

export const edgebandingListReducers = combineReducers({
    filters: edgebandingListFilterReducer
});
