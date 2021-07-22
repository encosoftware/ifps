
import { PagerModel } from '../../../../shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IInspectionFilterListViewModel } from '../../models/inspection.model';
import { InspectionAction, ActionTypes } from '../actions/inspection.actions';

const initialState: IInspectionFilterListViewModel = {
    date: undefined,
    delegation: '',
    report: '',
    stock: undefined,
    pager: new PagerModel()
};

function inspectionListFilterReducer(
    state: IInspectionFilterListViewModel = initialState,
    action: InspectionAction
): IInspectionFilterListViewModel {

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

export const inspectionListReducers = combineReducers({
    filters: inspectionListFilterReducer
});
