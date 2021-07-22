import { combineReducers } from '@ngrx/store';
import { ISortingListFilterViewModel } from '../../models/sorting.model';
import { PagerModel } from '../../../../shared/models/pager.model';
import { SortingAction, ActionTypes } from '../actions/sorting.actions';

const initialState: ISortingListFilterViewModel = {
    orderId: '',
    unitId: '',
    workingNr: '',
    date: undefined,
    workerName: '',
    pager: new PagerModel()
};

function sortingListFilterReducer(
    state: ISortingListFilterViewModel = initialState,
    action: SortingAction
): ISortingListFilterViewModel {

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

export const sortingListReducers = combineReducers({
    filters: sortingListFilterReducer
});
